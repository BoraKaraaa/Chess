using System;

public class Rook : ChessPiece
{
    private bool hasMoved = false;
    public bool HasMoved => hasMoved;
    
    public override void GetLegalAndCaptureMoves(ref Span<Move> legalMoves, ref int legalMoveIndex,
        ref Span<Move> captureMoves, ref int captureMoveIndex)
    { 
        ChessBoardAPI.CheckVerticalMoves(ref legalMoves, ref legalMoveIndex, ref captureMoves, ref captureMoveIndex,
            this, square, 7, eColor);
        ChessBoardAPI.CheckHorizontalMoves(ref legalMoves, ref legalMoveIndex, ref captureMoves, ref captureMoveIndex, 
            this, square, 7, eColor);
    }

    public override bool CanThreatSquare(Square targetSquare)
    {
        return ChessBoardAPI.CanVerticallyThreat(square, targetSquare) 
               || ChessBoardAPI.CanHorizontallyThreat(square, targetSquare);
    }
    
    protected override void OnAfterMoveCustomAction(Move move)
    {
        base.OnAfterMoveCustomAction(move);
        hasMoved = true;
    }
    
    public override string GetChessPieceNotationChar()
    {
        return "R";
    }
}
