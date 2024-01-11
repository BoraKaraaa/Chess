using UnityEngine;

public abstract class ChessBot : MonoBehaviour, IChessBot
{
    public abstract Move BestMove();
}
