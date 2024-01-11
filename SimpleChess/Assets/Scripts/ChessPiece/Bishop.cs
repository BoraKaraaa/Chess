
public class Bishop : ChessPiece
{
    public override (Square[], Square[]) LegalMoveSquares()
    {
        return ChessBoardAPI.CheckDiagonalMoves(square, 7, eColor);
    }
}
