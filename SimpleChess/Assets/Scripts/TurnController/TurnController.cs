using UnityEngine;
using System;
using System.Collections;

public enum EColor
{
    WHITE = 0,
    BLACK
}

public class TurnController : Singleton<TurnController>
{
    [Header("White Chess Bot")]
    [SerializeField] private ChessBot whiteChessBot;

    [Header("Black Chess Bot")] 
    [SerializeField] private ChessBot blackChessBot;

    public Action ChessMatchStarted;
    public Action ChessMatchResumed;
    public Action ChessMatchStopped;

    public Action<EColor> OnTurn;
    
    private EColor currentTurn = EColor.WHITE;
    public EColor CurrentTurn
    {
        get => currentTurn;
        set => currentTurn = value;
    }
    
    
    public void StartNewMatch()
    {
        // Restart Times
        ChessMatchStarted?.Invoke();
        Turn();
    }

    private void Turn()
    {
        OnTurn?.Invoke(currentTurn);
        
        if (currentTurn == EColor.WHITE)
        {
            if (whiteChessBot != null)
            {
                whiteChessBot.BestMove();
            }
            else
            {
                StartCoroutine(WaitToMove());
            }
        }
        else
        {
            if (blackChessBot != null)
            {
                blackChessBot.BestMove();
            }
            else
            {
                StartCoroutine(WaitToMove());
            }
        }

        if (!ChessAPI.IsCheckMate())
        {
            Turn();
        }
        else
        {
            // Finish Game
        }
    }

    public void MoveMaked()
    {
        StopAllCoroutines();
    }
    
    private IEnumerator WaitToMove()
    {
        while (true)
        {
            yield return null;
        }
    }

    public void PauseGame()
    {
        ChessMatchStopped?.Invoke();
    }

    public void ResumeGame()
    {
        ChessMatchResumed?.Invoke();
    }
}
