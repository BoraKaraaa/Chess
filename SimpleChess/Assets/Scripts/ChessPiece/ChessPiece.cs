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
    [SerializeField] private ETurn eTurn;
    [SerializeField] private EChessPiece eChessPiece;
}
