
public class Pawn : ChessPiece
{
    public override (Square[], Square[]) LegalMoveSquares()
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
}
