using System;

public class Queen : ChessPiece
{
    public override void GetLegalAndCaptureMoves(ref Span<Move> legalMoves, ref int legalMoveIndex,
        ref Span<Move> captureMoves, ref int captureMoveIndex)
    {
        ChessBoardAPI.CheckVerticalMoves(ref legalMoves, ref legalMoveIndex, ref captureMoves, ref captureMoveIndex,
            this, square, 7, eColor);
        ChessBoardAPI.CheckHorizontalMoves(ref legalMoves, ref legalMoveIndex, ref captureMoves, ref captureMoveIndex,
            this, square, 7, eColor);
        ChessBoardAPI.CheckDiagonalMoves(ref legalMoves, ref legalMoveIndex, ref captureMoves, ref captureMoveIndex, 
            this, square, 7, eColor);
    }

    public override bool CanThreatSquare(Square targetSquare)
    {
        return ChessBoardAPI.CanVerticallyThreat(square, targetSquare) 
               || ChessBoardAPI.CanHorizontallyThreat(square, targetSquare) 
               || ChessBoardAPI.CanDiagonallyThreat(square, targetSquare);
    }

    public override string GetChessPieceNotationChar()
    {
        return "Q";
    }
}
