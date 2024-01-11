
public class MyBot : ChessBot
{
    public override Move BestMove()
    {
        Move[] legalMoves = ChessAPI.GetLegalMoves();
        return legalMoves[0];
    }
}
