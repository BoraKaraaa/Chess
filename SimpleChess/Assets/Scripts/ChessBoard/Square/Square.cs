using UnityEngine;

public class Square : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    [SerializeField] private EColor eColor;

    [SerializeField] private int row;
    [SerializeField] private int col;

    [SerializeField] private string squareNotation;
    public string SquareNotation
    {
        get => squareNotation;
        set => squareNotation = value;
    }

    public SpriteRenderer SpriteRenderer
    {
        get => spriteRenderer;
        set => spriteRenderer = value;
    }
    public EColor EColor
    {
        get => eColor;
        set => eColor = value;
    }

    public int Row
    {
        get => row;
        set => row = value;
    }
    
    public int Col
    {
        get => col;
        set => col = value;
    }

    [SerializeField] private ChessPiece chessPiece;

    public ChessPiece ChessPiece
    {
        get => chessPiece;
        set => chessPiece = value;
    }

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
