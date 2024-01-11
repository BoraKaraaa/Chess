using UnityEngine;
using System;
using Munkur;

[RequireComponent( typeof(Collider2D) )]
public class Clickable2D : MonoBehaviour
{
    [SerializeField] private LayerMask layer;

    [SerializeField] private bool oneTime;

    [SerializeField] private bool considerUI;
    
    public bool OneTime => oneTime;
    
    public Action OnClicked;
    public Action OnUnclicked;

    private RaycastHit2D raycastHit2D;
    private Ray ray;

    private bool isClicked = false;
    
    private void Awake()
    { 
        MouseInputSystemManager.Instance.OnMouseLeftClicked += OnClick;
    }

    private void OnDestroy() 
    {
        MouseInputSystemManager.Instance.OnMouseLeftClicked -= OnClick;    
    }

    public virtual void OnClick(Vector2 mousePosition) 
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        raycastHit2D = Physics2D.GetRayIntersection(ray, Mathf.Infinity, layer);
        
        if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject == gameObject)
        {
            if (considerUI && StaticUtilitiesBase.IsPointerOverUIObject())
            {
                return;
            }

            isClicked = true;
            OnClicked?.Invoke();
            
            if (oneTime)
            {
                InputSystemManager.Instance.OnInputStarted -= OnClick; 
            }
        }
        else
        {
            if (isClicked)
            {
                isClicked = false;
                OnUnclicked?.Invoke();
            }
        }
    }
    
}
