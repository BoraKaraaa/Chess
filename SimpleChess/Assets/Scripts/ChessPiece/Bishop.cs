
public class Bishop : ChessPiece
{
    public override (Move[], Move[]) GetLegalMoves(bool controlCheck)
    {
        return ChessBoardAPI.CheckDiagonalMoves(this, square, 7, eColor, controlCheck);
    }

    public override string GetChessPieceNotationChar()
    {
        return "B";
    }
}
