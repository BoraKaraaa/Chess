
public class Knight : ChessPiece
{
    public override (Move[], Move[]) GetLegalMoves(bool controlCheck)
    {
        return ChessBoardAPI.CheckLMoves(this, square, eColor, controlCheck);
    }

    public override string GetChessPieceNotationChar()
    {
        return "N";
    }
}
