using System.Collections.Generic;

public static class ChessAPI
{
    public static List<ChessPiece> GetOpponentPieces()
    {
        return (TurnController.Instance.CurrentTurn == EColor.WHITE) ? GetBlackPieces() : GetWhitePieces();
    }
    
    public static List<ChessPiece> GetWhitePieces()
    {
        return ChessPieceSpawner.Instance.WhitePieces;
    }
    
    public static List<ChessPiece> GetBlackPieces()
    {
        return ChessPieceSpawner.Instance.BlackPieces;
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
    
    public static bool IsCheck()
    {
        Square kingSquare = GetMyKing().Square;

        foreach (ChessPiece opponentPiece in GetOpponentPieces())
        {
            foreach (Move move in opponentPiece.GetLegalMoves(false).Item2)
            {
                if (move.TargetSquare == kingSquare)
                {
                    return true;
                }
            }
        }

        return false;
    }
    
    public static bool IsCheckMate()
    {
        if (!IsCheck())
        {
            return false;
        }
        
        ChessPiece king = GetMyKing();

        if (king.GetLegalMoves().Item1.Length == 0)
        {
            return true;
        }

        return false;

        /*
        if (!IsCheck())
        {
            return false;
        }

        ChessPiece king = GetMyKing();

        foreach (Move move in king.GetLegalMoves().Item1)
        {
            ChessPiece capturedPiece = move.CapturedChessPiece;

            move.InitialSquare.ChessPiece = null;
            king.Square = move.TargetSquare;
            move.TargetSquare.ChessPiece = king;

            if (move.IsCaptured)
            {
                if (capturedPiece.EColor == EColor.WHITE)
                {
                    ChessPieceSpawner.Instance.WhitePieces.Remove(capturedPiece);
                }
                else
                {
                    ChessPieceSpawner.Instance.BlackPieces.Remove(capturedPiece);
                }

                capturedPiece.Square.ChessPiece = null;
            }

            if (!IsCheck())
            {
                UndoKingMove(move, king, capturedPiece);
                return false;
            }

            // Undo the move
            UndoKingMove(move, king, capturedPiece);
        }

        return true;
        */
    }

    private static void UndoKingMove(Move move, ChessPiece king, ChessPiece capturedPiece)
    {
        move.InitialSquare.ChessPiece = king;
        king.Square = move.InitialSquare;
        move.TargetSquare.ChessPiece = null;
            
        if (move.IsCaptured)
        {
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
