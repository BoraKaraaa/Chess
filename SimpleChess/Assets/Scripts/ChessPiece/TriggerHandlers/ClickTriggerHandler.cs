using System.Collections;
using UnityEngine;

public class ClickTriggerHandler : MonoBehaviour
{
    [SerializeField] private Clickable2D _clickable2D;

    private void Awake()
    {
        _clickable2D.OnClicked += OnChessPieceHolded;
        _clickable2D.OnUnclicked += OnChessPieceDropped;
    }

    private void OnDestroy()
    {
        _clickable2D.OnClicked -= OnChessPieceHolded;
        _clickable2D.OnUnclicked -= OnChessPieceDropped;
    }

    private void OnChessPieceHolded()
    {
        StartCoroutine(OnChessPieceDragged());
    }

    private void OnChessPieceDropped()
    {
        StopAllCoroutines();
    }

    private IEnumerator OnChessPieceDragged()
    {
        while (true)
        {
            yield return null;
        }
    }
}
