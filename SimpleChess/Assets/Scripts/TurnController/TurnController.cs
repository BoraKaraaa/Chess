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
    
    [Space(20)]
    
    [SerializeField] private TMP_Text gameResultText;
        
    [SerializeField] private List<GameObject> deactivateUIElements;
    
    public Action ChessMatchStarted;
    public Action ChessMatchResumed;
    public Action ChessMatchStopped;

    public Action ChessMatchedFinished;
    
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
    
    private Dictionary<string, int> fenHistory = new Dictionary<string, int>();

    private bool threeMoveRepetition = false;
    public bool ThreeMoveRepetition => threeMoveRepetition;

    private int fiftyMoveCounter = 0;
    public int FiftyMoveCounter => fiftyMoveCounter;
    
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

        StartGameCustomActions();
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

        if (!FENstringController.IsFenStringValid)
        {
            ChessBoard.Instance.ClearSquareRef();
        
            ChessPieceSpawner.Instance.ClearChessPieceRuntime();
            ChessPieceSpawner.Instance.InitChessPieces();   
        }

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

        UpdateBoardStateString();
        UpdateFiftyMoveCounter();
        
        if (ChessAPI.IsDraw())
        {
            gameResultText.text = "DRAW";
            AudioManager.Instance.CheckMateAudioSource.Play();
            
            EndGameCustomActions();
            ChessMatchedFinished?.Invoke();
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
            
            EndGameCustomActions();
            ChessMatchedFinished?.Invoke();
        }
    }

    public EColor NextTurnColor()
    {
        return currentTurn == EColor.WHITE ? EColor.BLACK : EColor.WHITE;
    }

    private void StartGameCustomActions()
    {
        foreach (var deactivateUIElement in deactivateUIElements)
        {
            deactivateUIElement.SetActive(false);
        }
    }

    private void EndGameCustomActions()
    {
        fiftyMoveCounter = 0;
        
        moveHistoryList.Clear();
        
        fenHistory.Clear();
        threeMoveRepetition = false;
        
        foreach (var deactivateUIElement in deactivateUIElements)
        {
            deactivateUIElement.SetActive(true);
        }
    }

    private void UpdateBoardStateString()
    {
        string boardFenString = FENstringController.Instance.GetCurrentBoardFenString();

        if (fenHistory.ContainsKey(boardFenString))
        {
            if (++fenHistory[boardFenString] >= 3)
            {
                threeMoveRepetition = true;
            }
        }
        else
        {
            fenHistory.Add(boardFenString, 1);
        }
    }
    
    private void UpdateFiftyMoveCounter()
    {
        Move lastMove = ChessAPI.GetLastMove();

        if (lastMove.IsCaptured || lastMove.MovedChessPiece.EChessPiece == EChessPiece.PAWN 
                                || lastMove.IsPromotion)
        {
            fiftyMoveCounter = 0;
        }

        fiftyMoveCounter++;
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
