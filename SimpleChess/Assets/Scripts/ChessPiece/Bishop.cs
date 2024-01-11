
public class Bishop : ChessPiece
{
    public override (Move[], Move[]) GetLegalMoves()
    {
        return ChessBoardAPI.CheckDiagonalMoves(this, square, 7, eColor);
    }

    public override string GetChessPieceNotationChar()
    {
        return "B";
    }
}
