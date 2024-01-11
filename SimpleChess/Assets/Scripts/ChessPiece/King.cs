
public class King : ChessPiece
{
    public override (Square[], Square[]) LegalMoveSquares()
    {
        return ChessBoardAPI.CheckAroundMoves(square, 1, eColor);
    }
}
