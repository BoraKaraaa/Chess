using UnityEngine;

public class MyBot : ChessBot
{
    public override Move BestMove()
    {
        Move[] legalMoves = ChessAPI.GetLegalMoves();
        return legalMoves[Random.Range(0, legalMoves.Length)];
    }
}
