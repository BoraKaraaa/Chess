using UnityEngine.EventSystems;
using UnityEngine;
using System;
using Munkur;

public enum EGameMode
{
    PLAYERvsBOT,
    BOTvsBOT
}

public class PlayButtonActivity : ButtonActivity
{
    [SerializeField] private EGameMode gameMode;
    
    public static Action<EGameMode> OnPlayButtonPressed;
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        OnPlayButtonPressed?.Invoke(gameMode);
    }
}
