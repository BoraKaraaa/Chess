using System;

public class Knight : ChessPiece
{
    public override void GetLegalAndCaptureMoves(ref Span<Move> legalMoves, ref int legalMoveIndex,
        ref Span<Move> captureMoves, ref int captureMoveIndex)
    {
        ChessBoardAPI.CheckLMoves(ref legalMoves, ref legalMoveIndex, ref captureMoves, ref captureMoveIndex,
            this, square, eColor);
    }

    public override bool CanThreatSquare(Square targetSquare)
    {
        return ChessBoardAPI.CanKnightThreat(square, targetSquare);
    }

    public override string GetChessPieceNotationChar()
    {
        return "N";
    }
}
