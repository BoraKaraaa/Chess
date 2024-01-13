
public class King : ChessPiece
{
    public override (Move[], Move[]) GetLegalMoves()
    {
        return ChessBoardAPI.CheckKingMoves(this, square, 1, eColor);
    }

    public override bool CanThreatSquare(Square targetSquare)
    {
        return ChessBoardAPI.CanKingThreat(square, targetSquare);
    }

    public override string GetChessPieceNotationChar()
    {
        return "K";
    }
}
