using System;
using DG.Tweening;
using UnityEngine;

public enum EChessPiece
{
    NONE,
    PAWN,
    KNIGHT,
    BISHOP,
    ROOK,
    QUEEN,
    KING
}

public abstract class ChessPiece : MonoBehaviour
{
    [SerializeField] protected EColor eColor;
    [SerializeField] protected EChessPiece eChessPiece;

    [SerializeField] protected int value;
    
    [SerializeField] protected Square square;

    private float moveDuration = 0.2f;

    public EColor EColor
    {
        get => eColor;
        set => eColor = value;
    }

    public EChessPiece EChessPiece
    {
        get => eChessPiece;
        set => eChessPiece = value;
    }

    public Square Square
    {
        get => square;
        set => square = value;
    }
    
    public abstract (Move[], Move[]) GetLegalMoves();
    
    public abstract bool CanThreatSquare(Square targetSquare);
    
    public void Move(Move move, Action<Move> OnMoved)
    {
        Square movedSquare = move.TargetSquare;
        
        square.ChessPiece = null;
        square = movedSquare;

        square.ChessPiece = this;

        OnBeforeMoveCustomAction(move);
        
        transform.DOMove(movedSquare.transform.position, moveDuration).OnComplete(() =>
        {
            OnAfterMoveCustomAction(move);
            OnMoved?.Invoke(move);
        });
    }
    
    protected virtual void OnBeforeMoveCustomAction(Move move)
    {
        
    }
    
    protected virtual void OnAfterMoveCustomAction(Move move)
    {
        
    }

    public void Captured()
    {
        if (eColor == EColor.WHITE)
        {
            ChessPieceSpawner.Instance.WhitePieces.Remove(this);
        }
        else
        {
            ChessPieceSpawner.Instance.BlackPieces.Remove(this);
        }

        square.ChessPiece = null;
        Destroy(this.gameObject);
    }

    public abstract string GetChessPieceNotationChar();
}
