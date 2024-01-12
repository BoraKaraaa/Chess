using System;
using UnityEngine;

public abstract class ChessBot : MonoBehaviour, IChessBot
{
    public abstract Move BestMove();

    public void Move(Action OnMoveMade)
    {
        Move bestMove = BestMove();
        
        this.WaitForSeconds(0.5f, () =>
        {
            if (bestMove.IsCaptured)
            {
                bestMove.CapturedChessPiece.Captured();
            }
            
            bestMove.MovedChessPiece.Move(bestMove.MovedSquare, OnMoveMade); 
        });
        
        // Debug
        Debug.Log(bestMove.MoveNotation);
    }
}
