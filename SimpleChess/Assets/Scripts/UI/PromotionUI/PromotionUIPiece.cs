using UnityEngine;

public class PromotionUIPiece : MonoBehaviour
{
    [SerializeField] private EColor eColor;
    [SerializeField] private EChessPiece eChessPiece;

    [SerializeField] private Clickable2D clickable2D;
    
    public void ActivatePieceClickable()
    {
        clickable2D.EnableCollider();
        clickable2D.OnClicked += OnClicked;
    }
    
    public void DeactivatePieceClickable()
    {
        clickable2D.DisableCollider();
        clickable2D.OnClicked -= OnClicked;
    }
    
    private void OnClicked()
    {
        PromotionUI.Instance.OnPieceClicked(eColor, eChessPiece);
    }
}
