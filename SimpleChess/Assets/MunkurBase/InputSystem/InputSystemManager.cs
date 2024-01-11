using UnityEngine;
//using UnityEngine.InputSystem;
using System;
//using System.Collections.Generic;
using TouchPhase = UnityEngine.TouchPhase;

namespace Munkur
{
    public class InputSystemManager : SingletonPersistent<InputSystemManager>
    {
        #region MouseAndTouchInput
        [Space(5)]
        [Header("Mouse And Touch Input")]
        public Action<Vector2> OnInputStarted;
        public Action<Vector2> OnInputContinued;
        public Action<Vector2> OnInputFinished;
    
        [SerializeField] private bool enableInputListener = false;
    
        public bool EnableInputListener => enableInputListener;

        [SerializeField] private bool isMobileInput;

        private void Update()
        {
            if (enableInputListener)
            {
                if (isMobileInput)
                {
                    HandleTouchInput();
                }
                else
                {
                    HandleMouseInput();
                }
            }
        }

        private void HandleTouchInput()
        {
            if(Input.touchCount > 0)
            {
                var theTouch = Input.GetTouch(0);

                if (theTouch.phase == TouchPhase.Began)
                {
                    OnInputStarted?.Invoke(theTouch.deltaPosition);
                }

                if (theTouch.phase == TouchPhase.Moved)
                {
                    OnInputContinued?.Invoke(theTouch.deltaPosition);   
                }

                if (theTouch.phase == TouchPhase.Ended)
                {
                    OnInputFinished?.Invoke(theTouch.deltaPosition);
                }
            }
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnInputStarted?.Invoke(Input.mousePosition);
            }
            else if(Input.GetMouseButtonUp(0))
            {
                OnInputFinished?.Invoke(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                OnInputContinued?.Invoke(Input.mousePosition);
            }
        }

        #endregion
        
        /*
        #region KeyboardAndJoystickInput

        [Header("Keyboard And Joystick Input")]
        [Space(10)]
        [SerializeField] private List<InputAction> _inputActions;

        #endregion
        */
    }
}