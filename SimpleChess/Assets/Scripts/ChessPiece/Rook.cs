
public class Rook : ChessPiece
{
    private bool hasMoved = false;
    public bool HasMoved => hasMoved;
    
    public override (Move[], Move[]) GetLegalMoves()
    {
        (Move[], Move[]) verticalMovesTuple = ChessBoardAPI
            .CheckVerticalMoves(this, square, 7, eColor);
        
        (Move[], Move[]) horizontalMovesTuple = ChessBoardAPI
            .CheckHorizontalMoves(this, square, 7, eColor);

        Move[] concanatedMoves = new Move[verticalMovesTuple.Item1.Length
                                          + horizontalMovesTuple.Item1.Length];
        
        Move[] concanatedCaptureMoves = new Move[verticalMovesTuple.Item2.Length
                                                 + horizontalMovesTuple.Item2.Length];

        verticalMovesTuple.Item1.CopyTo(concanatedMoves, 0);
        horizontalMovesTuple.Item1.CopyTo(concanatedMoves, verticalMovesTuple.Item1.Length);
        
        verticalMovesTuple.Item2.CopyTo(concanatedCaptureMoves, 0);
        horizontalMovesTuple.Item2.CopyTo(concanatedCaptureMoves, verticalMovesTuple.Item2.Length);
        
        return (concanatedMoves, concanatedCaptureMoves);
    }

    public override bool CanThreatSquare(Square targetSquare)
    {
        return ChessBoardAPI.CanVerticallyThreat(square, targetSquare) 
               || ChessBoardAPI.CanHorizontallyThreat(square, targetSquare);
    }
    
    protected override void OnAfterMoveCustomAction(Move move)
    {
        base.OnAfterMoveCustomAction(move);
        hasMoved = true;
    }
    
    public override string GetChessPieceNotationChar()
    {
        return "R";
    }
}
