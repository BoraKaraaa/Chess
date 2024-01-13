
public class Bishop : ChessPiece
{
    public override (Move[], Move[]) GetLegalMoves()
    {
        return ChessBoardAPI.CheckDiagonalMoves(this, square, 7, eColor);
    }

    public override bool CanThreatSquare(Square targetSquare)
    {
        return ChessBoardAPI.CanDiagonallyThreat(square, targetSquare);
    }

    public override string GetChessPieceNotationChar()
    {
        return "B";
    }
}
