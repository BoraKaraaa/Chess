
public class Knight : ChessPiece
{
    public override (Move[], Move[]) GetLegalMoves()
    {
        return ChessBoardAPI.CheckLMoves(this, square, eColor);
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
