
public class Queen : ChessPiece
{
    public override (Square[], Square[]) LegalMoveSquares()
    {
        (Square[], Square[]) verticalMovesTuple = ChessBoardAPI
            .CheckVerticalMoves(this, square, 7, eColor);
        
        (Square[], Square[]) horizontalMovesTuple = ChessBoardAPI
            .CheckHorizontalMoves(square, 7, eColor);
        
        (Square[], Square[]) diagonalMovesTuple = ChessBoardAPI
            .CheckDiagonalMoves(square, 7, eColor);

        Square[] concanatedMoves = new Square[verticalMovesTuple.Item1.Length
                                              + horizontalMovesTuple.Item1.Length
                                              + diagonalMovesTuple.Item1.Length];
        
        Square[] concanatedCaptureMoves = new Square[verticalMovesTuple.Item2.Length
                                                     + horizontalMovesTuple.Item2.Length
                                                     + diagonalMovesTuple.Item2.Length];

        verticalMovesTuple.Item1.CopyTo(concanatedMoves, 0);
        horizontalMovesTuple.Item1.CopyTo(concanatedMoves, verticalMovesTuple.Item1.Length);
        diagonalMovesTuple.Item1.CopyTo(concanatedMoves, verticalMovesTuple.Item1.Length 
                                                         + horizontalMovesTuple.Item1.Length);
        
        verticalMovesTuple.Item2.CopyTo(concanatedCaptureMoves, 0);
        horizontalMovesTuple.Item2.CopyTo(concanatedCaptureMoves, verticalMovesTuple.Item2.Length);
        diagonalMovesTuple.Item2.CopyTo(concanatedCaptureMoves, verticalMovesTuple.Item2.Length 
                                                                + horizontalMovesTuple.Item2.Length);
        
        return (concanatedMoves, concanatedCaptureMoves);
    }
}
