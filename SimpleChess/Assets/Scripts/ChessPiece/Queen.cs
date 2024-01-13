
public class Queen : ChessPiece
{
    public override (Move[], Move[]) GetLegalMoves(bool controlCheck)
    {
        (Move[], Move[]) verticalMovesTuple = ChessBoardAPI
            .CheckVerticalMoves(this, square, 7, eColor, controlCheck);
        
        (Move[], Move[]) horizontalMovesTuple = ChessBoardAPI
            .CheckHorizontalMoves(this, square, 7, eColor, controlCheck);
        
        (Move[], Move[]) diagonalMovesTuple = ChessBoardAPI
            .CheckDiagonalMoves(this, square, 7, eColor, controlCheck);

        Move[] concanatedMoves = new Move[verticalMovesTuple.Item1.Length
                                          + horizontalMovesTuple.Item1.Length
                                          + diagonalMovesTuple.Item1.Length];
        
        Move[] concanatedCaptureMoves = new Move[verticalMovesTuple.Item2.Length
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

    public override string GetChessPieceNotationChar()
    {
        return "Q";
    }
}
