using UnityEngine;
using System;

namespace Munkur
{
    public sealed class MouseInputSystemManager : SingletonPersistent<MouseInputSystemManager>
    {
        public Action<Vector2> OnMouseLeftClicked;
        //public Action<Vector2> OnMouseRightClicked;
        //public Action<Vector2> OnMouseScrollClicked;
        public Action<Vector2> OnMouseDragged;
        public Action<Vector2> OnMouseReleased;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseLeftClicked?.Invoke(Input.mousePosition);
            }
            else if(Input.GetMouseButtonUp(0))
            {
                OnMouseReleased?.Invoke(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                OnMouseDragged?.Invoke(Input.mousePosition);
            }
        }
    }
}
