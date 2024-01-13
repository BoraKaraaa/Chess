using UnityEngine;

public class ChessPieceRefs : Singleton<ChessPieceRefs>
{
    [Header("---White Pieces")]
    [Space(2)]
    
    [Header("WhitePawn")]
    public Sprite WhitePawnSprite;
    [Header("WhiteKnight")]
    public Sprite WhiteKnightSprite;
    [Header("WhiteBishop")]
    public Sprite WhiteBishopSprite;
    [Header("WhiteRook")]
    public Sprite WhiteRookSprite;
    [Header("WhiteQueen")]
    public Sprite WhiteQueenSprite;
    [Header("WhiteKing")]
    public Sprite WhiteKingSprite;
    
    [Space(10)]
    
    [Header("---Black Pieces")]
    [Space(2)]
    
    [Header("BlackPawn")]
    public Sprite BlackPawnSprite;
    [Header("BlackKnight")]
    public Sprite BlackKnightSprite;
    [Header("BlackBishop")]
    public Sprite BlackBishopSprite;
    [Header("BlackRook")]
    public Sprite BlackRookSprite;
    [Header("BlackQueen")]
    public Sprite BlackQueenSprite;
    [Header("BlackKing")]
    public Sprite BlackKingSprite;
}
