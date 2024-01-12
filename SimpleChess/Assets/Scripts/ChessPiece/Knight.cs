
public class Knight : ChessPiece
{
    public override (Move[], Move[]) GetLegalMoves()
    {
        return ChessBoardAPI.CheckLMoves(this, square, eColor);
    }

    public override string GetChessPieceNotationChar()
    {
        return "N";
    }
}
