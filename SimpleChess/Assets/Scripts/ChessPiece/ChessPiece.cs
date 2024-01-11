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

    protected Square square;

    private float moveDuration = 0.1f;

    public EColor EColor => eColor;
    public EChessPiece EChessPiece => eChessPiece;

    public Square Square
    {
        get => square;
        set => square = value;
    }
    
    public abstract (Move[], Move[]) GetLegalMoves();
    
    protected void Move(Square movedSquare)
    {
        // Init Move
        transform.DOMove(movedSquare.transform.position, moveDuration).OnComplete(() =>
        {
            
        });
    }

    protected void Captured()
    {
        if (eColor == EColor.WHITE)
        {
            ChessPieceSpawner.Instance.WhitePieces.Remove(this);
        }
        else
        {
            ChessPieceSpawner.Instance.BlackPieces.Remove(this);
        }
        
        Destroy(this.gameObject);
    }

    public abstract string GetChessPieceNotationChar();
}
