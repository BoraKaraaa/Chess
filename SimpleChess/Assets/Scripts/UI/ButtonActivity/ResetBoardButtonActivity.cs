using UnityEngine.EventSystems;
using Munkur;

public class ResetBoardButtonActivity : ButtonActivity
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        FENstringController.IsFenStringValid = false;
        
        ChessBoard.Instance.ClearSquareRef();
        
        ChessPieceSpawner.Instance.ClearChessPieceRuntime();
        ChessPieceSpawner.Instance.InitChessPieces();
    }
}
