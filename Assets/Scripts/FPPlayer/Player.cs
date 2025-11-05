using UnityEngine;
using UnityEngine.InputSystem;

namespace alidey
{
    [RequireComponent(typeof(FPController))]  
    public class Player : MonoBehaviour
    {
        #region Components
        [Header("Components")]
        [SerializeField] FPController FPController;
        #endregion

        #region Input Handling
        void OnMove(InputValue value)
        {
            FPController.MoveInput = value.Get<Vector2>();
        }
        void OnLook(InputValue value)
        {
            FPController.LookInput = value.Get<Vector2>();
        }

        void OnSprint(InputValue value)
        {
            FPController.SprintInput = value.isPressed;
        }
        void OnJump(InputValue value)
        {
            if (value.isPressed)
            {
                FPController.InitiateJump();
            }
        }
        #endregion
        
        #region Unity Methods
        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnValidate()
        {
            if (FPController == null)
            {
                FPController = GetComponent<FPController>();
            }
        }
        #endregion
    }
}
