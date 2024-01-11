using UnityEngine;

public class Square : MonoBehaviour
{
    [SerializeField] private EColor eColor;

    [SerializeField] private int row;
    [SerializeField] private int col;

    public int Row => row;
    public int Col => col;

    private ChessPiece chessPiece = null;
    public ChessPiece ChessPiece => chessPiece;

    public bool IsSquareEmpty()
    {
        return chessPiece == null;
    }

    public bool IsSecondRowForColor(EColor color)
    {
        return (row == 2 && color == EColor.WHITE) ||
               (row == 6 && color == EColor.BLACK);
    }
}
