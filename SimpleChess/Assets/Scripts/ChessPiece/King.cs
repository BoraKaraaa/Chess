using System;

public class King : ChessPiece
{
    private bool hasMoved = false;
    public bool HasMoved => hasMoved;
    
    public override void GetLegalAndCaptureMoves(ref Span<Move> legalMoves, ref int legalMoveIndex,
        ref Span<Move> captureMoves, ref int captureMoveIndex)
    {
        ChessBoardAPI.CheckKingMoves(ref legalMoves, ref legalMoveIndex, ref captureMoves, ref captureMoveIndex,
            this, square, 1, eColor);
    }

    public override bool CanThreatSquare(Square targetSquare)
    {
        return ChessBoardAPI.CanKingThreat(square, targetSquare);
    }
    
    protected override void OnBeforeMoveCustomAction(Move move)
    {
        base.OnBeforeMoveCustomAction(move);

        if (move.IsCastles)
        {
            Move rookMove = new Move
            {
                TargetSquare = move.IsCastlesKingSide
                    ? ChessBoardAPI.GetKingSideCastleRookSquare()
                    : ChessBoardAPI.GetQueenSideCastleRookSquare()
            };

            move.CastleRook.Move(rookMove, null);
        }
    }
    
    protected override void OnAfterMoveCustomAction(Move move)
    {
        base.OnAfterMoveCustomAction(move);
        hasMoved = true;
    }
    
    public override string GetChessPieceNotationChar()
    {
        return "K";
    }
}
