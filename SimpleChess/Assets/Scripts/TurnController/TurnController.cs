using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public enum EColor
{
    WHITE = 0,
    BLACK
}

public class TurnController : Singleton<TurnController>
{
    [SerializeField] private Player player;
    
    [Header("White Chess Bot")]
    [SerializeField] private ChessBot whiteChessBot;

    [Header("Black Chess Bot")] 
    [SerializeField] private ChessBot blackChessBot;

    [SerializeField] private TMP_Text gameResultText;
    
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

    public int TotalMoveCount => moveHistoryList.Count;

    private EGameMode gameMode;
    
    private void Awake()
    {
        PlayButtonActivity.OnPlayButtonPressed += OnPlayButtonPressed;
    }

    private void OnDestroy()
    {
        PlayButtonActivity.OnPlayButtonPressed -= OnPlayButtonPressed;
    }

    private void OnPlayButtonPressed(EGameMode gameMode)
    {
        this.gameMode = gameMode;
        
        if (gameMode == EGameMode.BOTvsBOT)
        {
            if (whiteChessBot != null)
            {
                whiteChessBot.EColor = EColor.WHITE;
            }

            if (blackChessBot != null)
            {
                blackChessBot.EColor = EColor.BLACK;
            }
        }
        else if (gameMode == EGameMode.PLAYERvsBOT)
        {
            player.EColor = EColor.WHITE;
            
            if (whiteChessBot != null)
            {
                whiteChessBot.EColor = EColor.BLACK;
            }
        }
        
        StartNewMatch();
    }

    private void StartNewMatch()
    {
        Restart();
        
        ChessMatchStarted?.Invoke();

        if (gameMode == EGameMode.PLAYERvsBOT)
        {
            PlayervsBot();
        }
        else
        {
            BotvsBot();
        }
    }

    private void Restart()
    {
        gameResultText.text = String.Empty;
        
        ChessBoard.Instance.ClearSquareRef();
        
        ChessPieceSpawner.Instance.ClearChessPieceRuntime();
        ChessPieceSpawner.Instance.InitChessPieces();
        
        moveHistoryList.Clear();

        currentTurn = EColor.WHITE;
    }
    
    private void BotvsBot()
    {
        if (currentTurn == EColor.WHITE)
        {
            whiteChessBot.Move(OnMoveMade);
        }
        else
        {
            blackChessBot.Move(OnMoveMade);
        }
    }
    
    private void PlayervsBot()
    {
        OnTurn?.Invoke(currentTurn);
            
        if (currentTurn == EColor.WHITE)
        {
            if (player.EColor == EColor.WHITE)
            {
                player.PlayerTurn(OnMoveMade);
            }
            else
            {
                whiteChessBot.Move(OnMoveMade);
            }
        }
        else
        {
            if (player.EColor == EColor.BLACK)
            {
                player.PlayerTurn(OnMoveMade);
            }
            else
            {
                whiteChessBot.Move(OnMoveMade);
            }
        }
    }

    private void OnMoveMade(Move move)
    {
        StopAllCoroutines();
        
        moveHistoryList.Add(move);

        currentTurn = NextTurnColor();

        if (ChessAPI.IsDraw())
        {
            gameResultText.text = "DRAW";
            AudioManager.Instance.CheckMateAudioSource.Play();
            return;
        }
        
        if (!ChessAPI.IsCheckMate())
        {
            if (gameMode == EGameMode.PLAYERvsBOT)
            {
                PlayervsBot();
            }
            else
            {
                BotvsBot();
            }
        }
        else
        {
            gameResultText.text = "" + currentTurn + " Lost";
            AudioManager.Instance.CheckMateAudioSource.Play();
        }
    }

    public EColor NextTurnColor()
    {
        return currentTurn == EColor.WHITE ? EColor.BLACK : EColor.WHITE;
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
