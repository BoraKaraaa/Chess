using System;

public class Bishop : ChessPiece
{
    public override void GetLegalAndCaptureMoves(ref Span<Move> legalMoves, ref int legalMoveIndex,
        ref Span<Move> captureMoves, ref int captureMoveIndex)
    {
        ChessBoardAPI.CheckDiagonalMoves(ref legalMoves, ref legalMoveIndex, ref captureMoves, ref captureMoveIndex,
            this, square, 7, eColor);
    }

    public override bool CanThreatSquare(Square targetSquare)
    {
        return ChessBoardAPI.CanDiagonallyThreat(square, targetSquare);
    }

    public override string GetChessPieceNotationChar()
    {
        return "B";
    }
}
