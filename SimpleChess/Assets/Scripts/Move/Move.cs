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
    public bool IsPromotion;

    public EChessPiece PromotionType;
    
    // TODO: Calculate IsChecked Value

    public Move(String moveNotation, Square initialSquare, Square targetSquare, ChessPiece movedChessPiece,
        ChessPiece capturedChessPiece, bool isChecked = false, bool isCaptured = false, bool isPromotion = false, 
        EChessPiece promotionType = EChessPiece.DEFAULT)

    {
        MoveNotation = moveNotation;
        InitialSquare = initialSquare;
        TargetSquare = targetSquare;
        MovedChessPiece = movedChessPiece;
        CapturedChessPiece = capturedChessPiece;
        IsChecked = isChecked;
        IsCaptured = isCaptured;
        IsPromotion = isPromotion;
        PromotionType = promotionType;
    }
}
