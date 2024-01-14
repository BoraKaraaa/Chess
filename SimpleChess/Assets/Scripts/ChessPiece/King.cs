
public class King : ChessPiece
{
    private bool hasMoved = false;
    public bool HasMoved => hasMoved;
    
    public override (Move[], Move[]) GetLegalMoves()
    {
        return ChessBoardAPI.CheckKingMoves(this, square, 1, eColor);
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
            Move rookMove = new Move();
            
            rookMove.TargetSquare = move.IsCastlesKingSide
                ? ChessBoardAPI.GetKingSideCastleRookSquare()
                : ChessBoardAPI.GetQueenSideCastleRookSquare();
            
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
