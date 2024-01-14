using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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

    private List<Move> moveHistoryList = new List<Move>();

    public List<Move> MoveHistoryList
    {
        get => moveHistoryList;
        set => moveHistoryList = value;
    }

    private void Start()
    {
        if (whiteChessBot != null)
        {
            whiteChessBot.EColor = EColor.WHITE;
        }

        if (blackChessBot != null)
        {
            blackChessBot.EColor = EColor.BLACK;
        }
        
        this.WaitForSeconds(1f, () =>
        {
            StartNewMatch();
        });
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
                whiteChessBot.Move(MoveMade);
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
                blackChessBot.Move(MoveMade);
            }
            else
            {
                StartCoroutine(WaitToMove());
            }
        }
    }

    private void MoveMade(Move move)
    {
        StopAllCoroutines();
        
        moveHistoryList.Add(move);

        currentTurn = NextTurnColor();

        if (ChessAPI.IsDraw())
        {
            // Finish Game
            Debug.Log("GAME FINISHED DRAW");
            return;
        }
        
        if (!ChessAPI.IsCheckMate())
        {
            Turn();
        }
        else
        {
            // Finish Game
            Debug.Log("GAME FINISHED " + currentTurn + " LOSE");
        }
    }

    public EColor NextTurnColor()
    {
        return currentTurn == EColor.WHITE ? EColor.BLACK : EColor.WHITE;
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
