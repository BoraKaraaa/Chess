using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ClickTriggerHandler : MonoBehaviour
{
    [SerializeField] private ChessPiece chessPiece;
    [SerializeField] private Clickable2D clickable2D;

    [SerializeField] private float speed;

    public ChessPiece ChessPiece
    {
        get => chessPiece;
        set => chessPiece = value;
    }
    
    private Vector3 mousePos;
    private Vector3 partialRes;

    private bool holded = false;

    private Vector3 initialPos;

    private void OnDestroy()
    {
        clickable2D.OnClicked -= OnChessPieceHolded;
        clickable2D.OnUnClicked -= OnChessPieceDropped;
    }

    public void EnableCollider()
    {
        clickable2D.EnableCollider();

        clickable2D.OnClicked += OnChessPieceHolded;
        clickable2D.OnUnClicked += OnChessPieceDropped;
    }

    public void DisableCollider()
    {
        clickable2D.DisableCollider();
        
        clickable2D.OnClicked -= OnChessPieceHolded;
        clickable2D.OnUnClicked -= OnChessPieceDropped;
    }
    
    private void OnChessPieceHolded()
    {
        holded = true;
        initialPos = chessPiece.transform.position;
        StartCoroutine(OnChessPieceDragged());
       
        Player.Instance.GetPlayerPieceLegalMoves(chessPiece);
    }

    private void OnChessPieceDropped()
    {
        if (holded)
        {
            holded = false;
            StopAllCoroutines();
            
            ChessBoard.Instance.DefaultAllSquares();

            var position = chessPiece.transform.position;
            Square square = ChessBoard.Instance.GetSquareByPosition(position.x, position.y);

            Move move = Player.Instance.IsSquareIncludedToLegalMoves(square);

            if (square != null && !move.IsNull)
            {
                Player.Instance.PlayerMove(move);
            }
            else
            {
                ReturnInitialPos();
            }
        }
    }
    
    public void ReturnInitialPos()
    {
        chessPiece.transform.DOMove(initialPos, 0.2f);
    }
    
    private IEnumerator OnChessPieceDragged()
    {
        while (true)
        {
            mousePos = Input.mousePosition;
            mousePos.z = 45;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            chessPiece.transform.position = Vector3.Lerp(chessPiece.transform.position, mousePos, speed * Time.deltaTime);;
            
            yield return null;
        }
    }
}
