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

    private ChessGameManager _localChessGameManager;
    
    private ChessPiece selectedChessPiece;
    
    public Action<Move> OnMoveMade;

    private Move lastMove;

    private void OnDestroy()
    {
        PromotionUI.Instance.OnPromotionPieceSelected -= OnPromotionPieceSelected;
    }

    public void PlayerTurn(ChessGameManager chessGameManager, Action<Move> OnMoveMade)
    {
        _localChessGameManager = chessGameManager;
        
        this.OnMoveMade = OnMoveMade;
        EnableChessPieceColliders();
    }

    public void GetPlayerPieceLegalMoves(ChessPiece chessPiece)
    {
        selectedChessPiece = chessPiece;

        (Move[], Move[]) moves = _localChessGameManager.GetLegalAndCaptureMoves();

        foreach (var legalMove in moves.Item1)
        {
            if (legalMove.MovedChessPiece == chessPiece)
            {
                ChessBoard.Instance.HighlightMoveSquares(legalMove.TargetSquare);
            }
        }
        
        foreach (var captureMove in moves.Item2)
        {
            if (captureMove.MovedChessPiece == chessPiece)
            {
                ChessBoard.Instance.HighlightCaptureSquares(captureMove.TargetSquare);
            }
        }
    }

    public void PlayerMove(Move move)
    {
        lastMove = move;
        
        DisableChessPieceColliders();
        
        if (move.IsPromotion)
        {
            PromotionUI.Instance.ActivatePieceUI(eColor, move.MovedChessPiece);
            PromotionUI.Instance.OnPromotionPieceSelected += OnPromotionPieceSelected;
            return;
        }
        
        if (move.IsCaptured)
        {
            move.CapturedChessPiece.Captured();
        }
        
        move.MovedChessPiece.Move(move, OnMoveMade);
    }
    

    public Move IsSquareIncludedToLegalMoves(Square square)
    {
        foreach (var legalMove in _localChessGameManager.GetLegalAndCaptureMoves().Item1)
        {
            if (legalMove.MovedChessPiece == selectedChessPiece && legalMove.TargetSquare == square)
            {
                return legalMove;
            }
        }

        return new Move(true);
    }

    private void OnPromotionPieceSelected(EChessPiece eChessPiece)
    {
        PromotionUI.Instance.OnPromotionPieceSelected -= OnPromotionPieceSelected;
        
        if (eChessPiece == EChessPiece.NONE)
        {
            lastMove.MovedChessPiece.ClickTriggerHandler.ReturnInitialPos();
            EnableChessPieceColliders();
        }
        else
        {
            if (lastMove.IsCaptured)
            {
                lastMove.CapturedChessPiece.Captured();
            }
            
            lastMove.PromotionType = eChessPiece;
            lastMove.MovedChessPiece.Move(lastMove, OnMoveMade);
        }
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
