using System;

public class Move
{
    public String MoveNotation;

    public Square InitialSquare;
    public Square TargetSquare;
    
    public ChessPiece MovedChessPiece;
    public ChessPiece CapturedChessPiece;

    public ChessPiece CastleRook;
    
    public bool IsChecked;
    public bool IsCaptured;
    public bool IsCastles;
    public bool IsCastlesKingSide;
    public bool IsPromotion;
    public bool IsEnpassant;
    
    public EChessPiece PromotionType;

    public Move(String moveNotation, Square initialSquare, Square targetSquare, ChessPiece movedChessPiece,
        ChessPiece capturedChessPiece, ChessPiece castleRook = null, bool isChecked = false, bool isCaptured = false, 
        bool isCastles = false, bool isCastlesKingSide = false, bool isPromotion = false, bool isEnpassant = false, 
        EChessPiece promotionType = EChessPiece.NONE)

    {
        MoveNotation = moveNotation;
        InitialSquare = initialSquare;
        TargetSquare = targetSquare;
        MovedChessPiece = movedChessPiece;
        CapturedChessPiece = capturedChessPiece;
        CastleRook = castleRook;
        IsChecked = isChecked;
        IsCaptured = isCaptured;
        IsCastles = isCastles;
        IsCastlesKingSide = isCastlesKingSide;
        IsPromotion = isPromotion;
        PromotionType = promotionType;
        IsEnpassant = isEnpassant;
    }

    public Move()
    {
        
    }
}
