using System.Collections.Generic;
using UnityEngine;
using System;

public class PromotionUI : Singleton<PromotionUI>
{
    [SerializeField] private Transform whitePieceHolder;
    [SerializeField] private Transform blackPieceHolder;

    [SerializeField] private Transform background;
    
    [SerializeField] private Clickable2D blackBackgroundClickable;

    [SerializeField] private List<PromotionUIPiece> promotionUIWhitePieces;
    [SerializeField] private List<PromotionUIPiece> promotionUIBlackPieces;
    
    public Action<EChessPiece> OnPromotionPieceSelected;

    private Vector3 whiteOffset = new Vector3(0.9f, -1.5f, 0f);
    private Vector3 blackOffset = new Vector3(0.9f, 1.5f, 0f);
    
    public void ActivatePieceUI(EColor eColor, ChessPiece chessPiece)
    {
        blackBackgroundClickable.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
        
        blackBackgroundClickable.EnableCollider();
        blackBackgroundClickable.OnClicked += OnBackgroundClicked;
        
        if (eColor == EColor.WHITE)
        {
            transform.position = chessPiece.transform.position + whiteOffset;
            
            ActivateWhitePieceClickables();
            whitePieceHolder.gameObject.SetActive(true);
        }
        else
        {
            transform.position = chessPiece.transform.position + blackOffset;
            
            ActivateBlackPieceClickables();
            blackPieceHolder.gameObject.SetActive(true);
        }
        
        gameObject.SetActive(true);
    }
    
    private void Deactivate()
    {
        blackBackgroundClickable.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
        
        whitePieceHolder.gameObject.SetActive(false);
        blackPieceHolder.gameObject.SetActive(false);
        
        gameObject.SetActive(false);
    }

    public void OnPieceClicked(EColor eColor, EChessPiece eChessPiece)
    {
        blackBackgroundClickable.OnClicked -= OnBackgroundClicked;
            
        if (eColor == EColor.WHITE)
        {
            DeactivateWhitePieceClickables();
        }
        else
        {
            DeactivateBlackPieceClickables();
        }
        
        Deactivate();
        OnPromotionPieceSelected?.Invoke(eChessPiece);
    }

    private void OnBackgroundClicked()
    {
        blackBackgroundClickable.OnClicked -= OnBackgroundClicked;
        
        if (Player.Instance.EColor == EColor.WHITE)
        {
            DeactivateWhitePieceClickables();
        }
        else
        {
            DeactivateBlackPieceClickables();
        }
        
        Deactivate();
        OnPromotionPieceSelected?.Invoke(EChessPiece.NONE);
    }

    private void ActivateWhitePieceClickables()
    {
        foreach (var promotionUIWhitePiece in promotionUIWhitePieces)
        {
            promotionUIWhitePiece.ActivatePieceClickable();
        }
    }
    
    private void DeactivateWhitePieceClickables()
    {
        foreach (var promotionUIWhitePiece in promotionUIWhitePieces)
        {
            promotionUIWhitePiece.DeactivatePieceClickable();
        }
    }
    
    private void ActivateBlackPieceClickables()
    {
        foreach (var promotionUIBlackPiece in promotionUIBlackPieces)
        {
            promotionUIBlackPiece.ActivatePieceClickable();
        }
    }
    
    private void DeactivateBlackPieceClickables()
    {
        foreach (var promotionUIBlackPiece in promotionUIBlackPieces)
        {
            promotionUIBlackPiece.DeactivatePieceClickable();
        }
    }
}
