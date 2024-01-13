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

    public static ChessPiece[] GetAllChessPieces()
    {
        List<ChessPiece> allChessPiecesList = new List<ChessPiece>();
        allChessPiecesList.AddRange(GetWhitePieces());
        allChessPiecesList.AddRange(GetBlackPieces());

        return allChessPiecesList.ToArray();
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
    
    public static Move GetLastMove()
    {
        if (TurnController.Instance.MoveHistoryList.Count == 0)
        {
            return null;
        }
        
        return TurnController.Instance.MoveHistoryList[^1];
    }
    
    public static void MakeAbstractMove(Move move)
    {
        move.InitialSquare.ChessPiece = null;
        move.MovedChessPiece.Square = move.TargetSquare;
        move.TargetSquare.ChessPiece = move.MovedChessPiece;

        if (move.IsCaptured)
        {
            ChessPiece capturedPiece = move.CapturedChessPiece;

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

    public static void UndoAbstractMove(Move move)
    {
        move.InitialSquare.ChessPiece = move.MovedChessPiece;
        move.MovedChessPiece.Square = move.InitialSquare;
        move.TargetSquare.ChessPiece = null;

        if (move.IsCaptured)
        {
            ChessPiece capturedPiece = move.CapturedChessPiece;

            if (capturedPiece.EColor == EColor.WHITE)
            {
                ChessPieceSpawner.Instance.WhitePieces.Add(capturedPiece);
            }
            else
            {
                ChessPieceSpawner.Instance.BlackPieces.Add(capturedPiece);
            }

            capturedPiece.Square.ChessPiece = capturedPiece;
        }
    }

    public static bool IsMoveCheck(Move move)
    {
        MakeAbstractMove(move);

        if (IsCheckOpp())
        {
            UndoAbstractMove(move);
            return true;
        }
        
        UndoAbstractMove(move);
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
    
    public static bool IsCheckMate()
    {
        if (!IsCheckMe())
        {
            return false;
        }
        
        ChessPiece king = GetMyKing();

        if (king.GetLegalMoves().Item1.Length == 0)
        {
            foreach (var chessPiece in GetMyPieces())
            {
                if (chessPiece.EChessPiece != EChessPiece.KING)
                {
                    foreach (var move in chessPiece.GetLegalMoves().Item1)
                    {
                        // Make Move
                        MakeAbstractMove(move);
                        
                        // Control Check

                        if (!IsCheckMe())
                        {
                            return false;
                        }
                        
                        // Undo Move
                        UndoAbstractMove(move);
                    }       
                }
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    public static bool IsDraw()
    {
        if (IsCheckMe())
        {
            return false;
        }

        foreach (var chessPiece in GetMyPieces())
        {
            if (chessPiece.GetLegalMoves().Item1.Length != 0)
            {
                return false;
            }
        }

        return true;
    }
    
    public static (Move[], Move[]) GetLegalAndCaptureMoves()
    {
        List<ChessPiece> chessPieces;
        
        if (TurnController.Instance.CurrentTurn == EColor.WHITE)
        {
            chessPieces = GetWhitePieces();
        }
        else
        {
            chessPieces = GetBlackPieces();
        }

        List<Move> totalLegalMoves = new List<Move>();
        List<Move> totalLegalCaptureMoves = new List<Move>();
        
        foreach (ChessPiece chessPiece in chessPieces)
        {
            (Move[], Move[]) legalMoves = chessPiece.GetLegalMoves();
            
            totalLegalMoves.AddRange(legalMoves.Item1);
            totalLegalCaptureMoves.AddRange(legalMoves.Item2);
        }
        
        return (totalLegalMoves.ToArray(), totalLegalCaptureMoves.ToArray());
    }

    public static Move[] GetLegalMoves()
    {
        List<ChessPiece> chessPieces;
        
        if (TurnController.Instance.CurrentTurn == EColor.WHITE)
        {
            chessPieces = GetWhitePieces();
        }
        else
        {
            chessPieces = GetBlackPieces();
        }
        
        List<Move> totalLegalMoves = new List<Move>();
        
        foreach (ChessPiece chessPiece in chessPieces)
        {
            (Move[], Move[]) legalMoves = chessPiece.GetLegalMoves();
            
            totalLegalMoves.AddRange(legalMoves.Item1);
        }

        return totalLegalMoves.ToArray();
    }

    public static Move[] GetCaptureMoves()
    {
        List<ChessPiece> chessPieces;
        
        if (TurnController.Instance.CurrentTurn == EColor.WHITE)
        {
            chessPieces = GetWhitePieces();
        }
        else
        {
            chessPieces = GetBlackPieces();
        }
        
        List<Move> totalLegalCaptureMoves = new List<Move>();
        
        foreach (ChessPiece chessPiece in chessPieces)
        {
            (Move[], Move[]) legalMoves = chessPiece.GetLegalMoves();

            totalLegalCaptureMoves.AddRange(legalMoves.Item2);
        }
        
        return totalLegalCaptureMoves.ToArray();
    }
    
}
