
public class King : ChessPiece
{
    public override (Move[], Move[]) GetLegalMoves()
    {
        return ChessBoardAPI.CheckAroundMoves(this, square, 1, eColor);
    }

    public override string GetChessPieceNotationChar()
    {
        return "K";
    }
}
