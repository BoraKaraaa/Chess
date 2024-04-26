using UnityEngine;

public class EvilBot : ChessBot
{
    public override Move BestMove(ChessGameManager chessGameManager)
    {
        Move[] legalMoves = chessGameManager.GetLegalAndCaptureMoves().Item1;
        return legalMoves[Random.Range(0, legalMoves.Length)];
    }
}
