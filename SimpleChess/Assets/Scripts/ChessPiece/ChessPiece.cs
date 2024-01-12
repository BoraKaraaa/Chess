using System;
using DG.Tweening;
using UnityEngine;

public enum EChessPiece
{
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

    public EColor EColor => eColor;
    public EChessPiece EChessPiece => eChessPiece;

    public Square Square
    {
        get => square;
        set => square = value;
    }
    
    public abstract (Move[], Move[]) GetLegalMoves();
    
    public void Move(Square movedSquare, Action OnMoved)
    {
        square.ChessPiece = null;
        square = movedSquare;
        
        transform.DOMove(movedSquare.transform.position, moveDuration).OnComplete(() =>
        {
            OnMoved?.Invoke();
        });
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
