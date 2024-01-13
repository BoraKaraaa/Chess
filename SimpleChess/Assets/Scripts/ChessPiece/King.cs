
public class King : ChessPiece
{
    public override (Move[], Move[]) GetLegalMoves(bool controlCheck)
    {
        return ChessBoardAPI.CheckAroundMoves(this, square, 1, eColor, controlCheck);
    }

    public override string GetChessPieceNotationChar()
    {
        return "K";
    }
}
