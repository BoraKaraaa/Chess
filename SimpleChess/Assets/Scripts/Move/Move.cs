using System;

public class Move
{
    public String MoveNotation;

    public Square InitialSquare;
    public Square MovedSquare;
    
    public ChessPiece MovedChessPiece;
    public ChessPiece CapturedChessPiece;
    
    public bool IsChecked;
    public bool IsCaptured;
    
    // TODO: Calculate IsChecked Value
    
    public Move(String moveNotation, Square initialSquare, Square movedSquare, ChessPiece movedChessPiece,
        ChessPiece capturedChessPiece, bool isChecked, bool isCaptured)
    {
        MoveNotation = moveNotation;
        InitialSquare = initialSquare;
        MovedSquare = movedSquare;
        MovedChessPiece = movedChessPiece;
        CapturedChessPiece = capturedChessPiece;
        IsChecked = isChecked;
        IsCaptured = isCaptured;
    }
}
