using System;

public class Move
{
    public String MoveNotation;

    public Square InitialSquare;
    public Square TargetSquare;
    
    public ChessPiece MovedChessPiece;
    public ChessPiece CapturedChessPiece;
    
    public bool IsChecked;
    public bool IsCaptured;
    public bool IsCastles;
    public bool IsPromotion;
    public bool IsEnpassant;
    
    public EChessPiece PromotionType;

    public Move(String moveNotation, Square initialSquare, Square targetSquare, ChessPiece movedChessPiece,
        ChessPiece capturedChessPiece, bool isChecked = false, bool isCaptured = false, bool isCastles = false,
        bool isPromotion = false, bool isEnpassant = false, EChessPiece promotionType = EChessPiece.NONE)

    {
        MoveNotation = moveNotation;
        InitialSquare = initialSquare;
        TargetSquare = targetSquare;
        MovedChessPiece = movedChessPiece;
        CapturedChessPiece = capturedChessPiece;
        IsChecked = isChecked;
        IsCaptured = isCaptured;
        IsCastles = isCastles;
        IsPromotion = isPromotion;
        PromotionType = promotionType;
        IsEnpassant = isEnpassant;
    }

    public Move()
    {
        
    }
}
