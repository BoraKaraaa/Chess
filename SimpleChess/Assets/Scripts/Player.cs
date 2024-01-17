using System;
using System.Collections.Generic;

public class Player : Singleton<Player>
{
    private EColor eColor;
    public EColor EColor
    {
        get => eColor;
        set => eColor = value;
    }

    private Dictionary<ChessPiece, (Move[], Move[])> legalMoveDictionary = new Dictionary<ChessPiece, (Move[], Move[])>();

    private ChessPiece selectedChessPiece;
    
    public Action<Move> OnMoveMade;
    
    public void PlayerTurn(Action<Move> OnMoveMade)
    {
        this.OnMoveMade = OnMoveMade;
        EnableChessPieceColliders();
    }

    public void GetPlayerPieceLegalMoves(ChessPiece chessPiece)
    {
        selectedChessPiece = chessPiece;

        if (!legalMoveDictionary.ContainsKey(chessPiece))
        { 
            legalMoveDictionary.Add(chessPiece, chessPiece.GetLegalMoves());
        }

        foreach (var legalMove in legalMoveDictionary[chessPiece].Item1)
        {
            ChessBoard.Instance.HighlightMoveSquares(legalMove.TargetSquare);
        }

        foreach (var captureMove in legalMoveDictionary[chessPiece].Item2)
        {
            ChessBoard.Instance.HighlightCaptureSquares(captureMove.TargetSquare);
        }
    }

    public void PlayerMove(Move move)
    {
        legalMoveDictionary.Clear();
        
        DisableChessPieceColliders();
        
        if (move.IsCaptured)
        {
            move.CapturedChessPiece.Captured();
        }
        
        move.MovedChessPiece.Move(move, OnMoveMade); 
    }

    public Move IsSquareIncludedToLegalMoves(Square square)
    {
        foreach (var legalMove in legalMoveDictionary[selectedChessPiece].Item1)
        {
            if (legalMove.TargetSquare == square)
            {
                return legalMove;
            }
        }

        return null;
    }
    
    private void EnableChessPieceColliders()
    {
        if (eColor == EColor.WHITE)
        {
            foreach (var whiteChessPiece in ChessAPI.GetWhitePieces())
            {
                whiteChessPiece.ClickTriggerHandler.EnableCollider();
            }
        }
        else
        {
            foreach (var blackChessPiece in ChessAPI.GetBlackPieces())
            {
                blackChessPiece.ClickTriggerHandler.EnableCollider();
            }
        }
    }
    
    private void DisableChessPieceColliders()
    {
        if (eColor == EColor.WHITE)
        {
            foreach (var whiteChessPiece in ChessAPI.GetWhitePieces())
            {
                whiteChessPiece.ClickTriggerHandler.DisableCollider();
            }
        }
        else
        {
            foreach (var blackChessPiece in ChessAPI.GetBlackPieces())
            {
                blackChessPiece.ClickTriggerHandler.DisableCollider();
            }
        }
    }
}
