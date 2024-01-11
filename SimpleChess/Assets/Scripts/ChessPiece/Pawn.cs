
public class Pawn : ChessPiece
{
    public override (Move[], Move[]) GetLegalMoves()
    {
        if (square.IsSecondRowForColor(eColor))
        {
            return ChessBoardAPI.CheckVerticalMoves(this, square, 2, eColor);
        }
        else
        {
            return ChessBoardAPI.CheckVerticalMoves(this, square, 1, eColor);
        }
    }

    public override string GetChessPieceNotationChar()
    {
        return "";
    }
}
