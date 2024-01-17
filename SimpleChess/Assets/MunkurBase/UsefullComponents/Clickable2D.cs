using UnityEngine;
using System;
using Munkur;

[RequireComponent( typeof(Collider2D) )]
public class Clickable2D : MonoBehaviour
{
    [SerializeField] private Collider2D colliderr2D;
    
    [SerializeField] private LayerMask layer;

    [SerializeField] private bool considerUI;
    
    public Action OnClicked;
    public Action OnUnClicked;

    private RaycastHit2D raycastHit2D;
    private Ray ray;

    private void OnDestroy()
    {
        MouseInputSystemManager.Instance.OnMouseLeftClicked -= OnClick;  
        MouseInputSystemManager.Instance.OnMouseLeftClickReleased -= OnUncliked;
    }

    public void EnableCollider()
    {
        colliderr2D.enabled = true;
        MouseInputSystemManager.Instance.OnMouseLeftClicked += OnClick;
    }

    public void DisableCollider()
    {
        colliderr2D.enabled = false;
        MouseInputSystemManager.Instance.OnMouseLeftClicked -= OnClick;  
    }
    
    public virtual void OnClick(Vector2 mousePos) 
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        raycastHit2D = Physics2D.GetRayIntersection(ray, Mathf.Infinity, layer);
        
        if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject == gameObject)
        {
            if (considerUI && StaticUtilitiesBase.IsPointerOverUIObject())
            {
                return;
            }
            
            OnClicked?.Invoke();
            MouseInputSystemManager.Instance.OnMouseLeftClickReleased += OnUncliked;
        }
    }
    
    private void OnUncliked(Vector2 mousePos)
    {
        MouseInputSystemManager.Instance.OnMouseLeftClickReleased -= OnUncliked;
        OnUnClicked?.Invoke();
    }
}
