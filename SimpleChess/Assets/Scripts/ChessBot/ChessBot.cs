using System;
using UnityEngine;

public abstract class ChessBot : MonoBehaviour, IChessBot
{
    [HideInInspector] public EColor EColor;
    
    public abstract Move BestMove();

    public void Move(Action<Move> OnMoveMade)
    {
        Move bestMove = BestMove();
        
        this.WaitForSeconds(0.1f, () =>
        {
            if (bestMove.IsCaptured)
            {
                bestMove.CapturedChessPiece.Captured();
            }
            
            bestMove.MovedChessPiece.Move(bestMove, OnMoveMade); 
        });
        
        // Debug
        //Debug.Log(bestMove.MoveNotation);
    }
}
