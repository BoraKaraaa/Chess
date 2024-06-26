using System.Collections.Generic;

public static class ChessAPI
{
    public static bool IsWhiteToMove()
    {
        return TurnController.Instance.CurrentTurn == EColor.WHITE;
    }
    
    public static List<ChessPiece> GetOpponentPieces()
    {
        return (TurnController.Instance.CurrentTurn == EColor.WHITE) ? GetBlackPieces() : GetWhitePieces();
    }

    public static List<ChessPiece> GetMyPieces()
    {
        return (TurnController.Instance.CurrentTurn == EColor.BLACK) ? GetBlackPieces() : GetWhitePieces();
    }

    public static List<ChessPiece> GetWhitePieces()
    {
        return ChessPieceSpawner.Instance.WhitePieces;
    }

    public static List<ChessPiece> GetBlackPieces()
    {
        return ChessPieceSpawner.Instance.BlackPieces;
    }
    
    public static List<ChessPiece> GetAllChessPieces()
    {
        List<ChessPiece> allChessPiecesList = new List<ChessPiece>();
        allChessPiecesList.AddRange(GetWhitePieces());
        allChessPiecesList.AddRange(GetBlackPieces());

        return allChessPiecesList;
    }

    public static ChessPiece GetOpponentKing()
    {
        return (TurnController.Instance.CurrentTurn == EColor.WHITE)
            ? ChessPieceSpawner.Instance.BlackKingInstance
            : ChessPieceSpawner.Instance.WhiteKingInstance;
    }

    public static ChessPiece GetMyKing()
    {
        return (TurnController.Instance.CurrentTurn != EColor.WHITE)
            ? ChessPieceSpawner.Instance.BlackKingInstance
            : ChessPieceSpawner.Instance.WhiteKingInstance;
    }

    public static ChessPiece GetLeftRook()
    {
        return (TurnController.Instance.CurrentTurn != EColor.WHITE)
            ? ChessPieceSpawner.Instance.BlackLeftRook
            : ChessPieceSpawner.Instance.WhiteLeftRook;
    }

    public static ChessPiece GetRightRook()
    {
        return (TurnController.Instance.CurrentTurn != EColor.WHITE)
            ? ChessPieceSpawner.Instance.BlackRightRook
            : ChessPieceSpawner.Instance.WhiteRightRook;
    }
    
    public static Move GetLastMove()
    {
        if (TurnController.Instance.MoveHistoryList.Count == 0)
        {
            return new Move(true);
        }
        
        return TurnController.Instance.MoveHistoryList[^1];
    }

    public static bool CanKingCastle()
    {
        if (IsCheckMe())
        {
            return false;
        }
        
        King myKing = (King)GetMyKing();
        return !myKing.HasMoved;
    }
    
    public static bool IsShortCastlingPossible()
    {
        int row = (TurnController.Instance.CurrentTurn == EColor.WHITE) ? 0 : 7;
        
        for (int col = 5; col <= 6; col++)
        {
            if (!ChessBoardAPI.IsSquareEmpty(row, col) || ChessBoardAPI.IsSquareUnderThreat(row, col))
            {
                return false;
            }
        }
        
        Rook rook = (Rook)GetRightRook();
        
        if (rook != null && !rook.HasMoved)
        {
            return true;
        }

        return false;
    }

    public static bool IsLongCastlingPossible()
    {
        int row = (TurnController.Instance.CurrentTurn == EColor.WHITE) ? 0 : 7;
        
        for (int col = 1; col <= 3; col++)
        {
            if (!ChessBoardAPI.IsSquareEmpty(row, col) || ChessBoardAPI.IsSquareUnderThreat(row, col))
            {
                return false;
            }
        }
        
        Rook rook = (Rook)GetLeftRook();
        
        if (rook != null && !rook.HasMoved)
        {
            return true;
        }

        return false;
    }

    public static float GetLateGameRate()
    {
        return 1 - ((ChessPieceSpawner.Instance.WhitePieces.Count + ChessPieceSpawner.Instance.BlackPieces.Count) / 32);
    }
    
    public static void MakeAbstractMove(ref Move move)
    {
        TurnController.Instance.CurrentTurn = TurnController.Instance.NextTurnColor();
        TurnController.Instance.MoveHistoryList.Add(move);
        
        move.InitialSquare.ChessPiece = null;
        move.MovedChessPiece.Square = move.TargetSquare;
        move.TargetSquare.ChessPiece = move.MovedChessPiece;

        if (move.IsCaptured)
        {
            ChessPiece capturedPiece = move.CapturedChessPiece;
            capturedPiece.Square = null;
            
            if (capturedPiece.EColor == EColor.WHITE)
            {
                ChessPieceSpawner.Instance.WhitePieces.Remove(capturedPiece);
            }
            else
            {
                ChessPieceSpawner.Instance.BlackPieces.Remove(capturedPiece);
            }
        }
    }

    public static void UndoAbstractMove(ref Move move)
    {
        TurnController.Instance.CurrentTurn = TurnController.Instance.NextTurnColor();
        TurnController.Instance.MoveHistoryList.RemoveAt(TurnController.Instance.MoveHistoryList.Count-1);
        
        move.InitialSquare.ChessPiece = move.MovedChessPiece;
        move.MovedChessPiece.Square = move.InitialSquare;
        move.TargetSquare.ChessPiece = null;

        if (move.IsCaptured)
        {
            ChessPiece capturedPiece = move.CapturedChessPiece;
            capturedPiece.Square = move.TargetSquare;
            
            if (capturedPiece.EColor == EColor.WHITE)
            {
                ChessPieceSpawner.Instance.WhitePieces.Add(capturedPiece);
            }
            else
            {
                ChessPieceSpawner.Instance.BlackPieces.Add(capturedPiece);
            }

            move.TargetSquare.ChessPiece = capturedPiece;
        }
    }

    public static bool IsMoveCheck(ref Move move)
    {
        MakeAbstractMove(ref move);
        
        if (IsCheckMe())
        {
            UndoAbstractMove(ref move);
            return true;
        }
        
        UndoAbstractMove(ref move);
        return false;
    }
    
    public static bool IsCheckOpp()
    {
        Square kingSquare = GetOpponentKing().Square;

        foreach (ChessPiece myPiece in GetMyPieces())
        {
            if (myPiece.CanThreatSquare(kingSquare))
            {
                return true;
            }
        }
        
        return false;
    }
    
    public static bool IsCheckMe()
    {
        Square kingSquare = GetMyKing().Square;

        foreach (ChessPiece opponentPiece in GetOpponentPieces())
        {
            if (opponentPiece.CanThreatSquare(kingSquare))
            {
                return true;
            }
        }
        
        return false;
    }
}
