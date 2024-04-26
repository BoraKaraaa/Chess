using System;

public class ChessGameManager
{
    private readonly Move[] legalMoveArea;
    private readonly Move[] captureMoveArea;
    
    private Move[] cachedLegalMoves;
    private Move[] cachedCaptureMoves;

    private int legalMoveIndex;
    private int captureMoveIndex;
    
    private bool isMovesCached;

    private int cachedMoveCount;
    private bool isMoveCountCached;
    
    private const int MAX_MOVES = 218;
    private const int MAX_CAPTURES = 128;

    public ChessGameManager()
    {
        legalMoveArea = new Move[MAX_MOVES];
        captureMoveArea = new Move[MAX_CAPTURES];

        cachedLegalMoves = Array.Empty<Move>();
        cachedCaptureMoves = Array.Empty<Move>();
        
        legalMoveIndex = 0;
        captureMoveIndex = 0;
        
        isMovesCached = false;
    }

    public void PositionChanged()
    {
        isMovesCached = false;
        isMoveCountCached = false;
        
        legalMoveIndex = 0;
        captureMoveIndex = 0;
    }
    
    public (Move[], Move[]) GetLegalAndCaptureMoves()
    {
        if (!isMovesCached)
        {
            Span<Move> legalMoves = legalMoveArea.AsSpan();
            Span<Move> captureMoves = captureMoveArea.AsSpan();
            
            foreach (ChessPiece chessPiece in ChessAPI.GetMyPieces())
            {
                chessPiece.GetLegalAndCaptureMoves(ref legalMoves, ref legalMoveIndex, ref captureMoves, ref captureMoveIndex);
            }

            legalMoves = legalMoves.Slice(0, legalMoveIndex);
            captureMoves = captureMoves.Slice(0, captureMoveIndex);
            
            cachedLegalMoves = legalMoves.ToArray();
            cachedCaptureMoves = captureMoves.ToArray();
            
            cachedMoveCount = legalMoves.Length;
            
            isMovesCached = true;

            isMoveCountCached = true;
        }

        return (cachedLegalMoves, cachedCaptureMoves);
    }
    
    public bool IsCheckMate()
    {
        if (!ChessAPI.IsCheckMe())
        {
            return false;
        }
        
        if (isMoveCountCached)
        {
            GetLegalAndCaptureMoves();
        }

        if (cachedMoveCount != 0)
        {
            return false;
        }

        return true;
    }
    
    public bool IsDraw()
    {
        if (TurnController.Instance.FiftyMoveCounter >= 100)
        {
            return true;
        }

        if (TurnController.Instance.ThreeMoveRepetition)
        {
            return true;
        }
        
        if (ChessAPI.IsCheckMe())
        {
            return false;
        }
        
        if (GetLegalAndCaptureMoves().Item1.Length != 0)
        {
            return false;
        }
        
        return true;
    }
    
    public void MakeAbstractMove(ref Move move)
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

        PositionChanged();
    }

    public void UndoAbstractMove(ref Move move)
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

        PositionChanged();
    }
}
