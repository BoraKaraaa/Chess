using System;
using UnityEngine;

public abstract class ChessBot : MonoBehaviour, IChessBot
{
    [HideInInspector] public EColor EColor;
    [SerializeField] private bool enableNotationDebug;
    
    public abstract Move BestMove(ChessGameManager chessGameManager);

    public void Move(ChessGameManager chessGameManager, Action<Move> OnMoveMade)
    {
        Move bestMove = BestMove(chessGameManager);
        
        this.WaitForSeconds(0.1f, () =>
        {
            if (bestMove.IsCaptured)
            {
                bestMove.CapturedChessPiece.Captured();
            }
            
            bestMove.MovedChessPiece.Move(bestMove, OnMoveMade); 
        });

        if (enableNotationDebug)
        {
           //Debug.Log(bestMove.MoveNotation);
        }
    }
}
