using UnityEngine;
using Unity.Cinemachine;

namespace alidey
{
    [RequireComponent(typeof(CharacterController))]
    public class FPController : MonoBehaviour
    {
        #region Movement
        [Header("Movement Parameters")]
        [SerializeField] public float MaxSpeed => SprintInput ? SprintSpeed : WalkSpeed;
        [SerializeField] public float Acceleration = 15f;
        [SerializeField] float WalkSpeed = 3.5f;
        [SerializeField] float SprintSpeed = 18f;

        [Space(15)]
        [Tooltip("How high the character can jump.")]
        [SerializeField] float JumpHeight = 2f;

        public bool Sprinting
        {
            get
            {
                return SprintInput && CurrentSpeed > 0.1f;
            }
        }
        #endregion

        #region Looking
        [Header("Looking Parameters")]
        public Vector2 LookSensitivity = new Vector2(0.1f, 0.1f);
        public float PitchLimit = 85f;
        [SerializeField] float currentPitch = 0f;

        public float CurrentPitch
        {
            get => currentPitch;
            set
            {
                currentPitch = Mathf.Clamp(value, -PitchLimit, PitchLimit);
            }
        }
        #endregion

        #region Camera
        [Header("Camera Parameters")]
        [SerializeField] float CameraFOVSmoothing = 1f;
        [SerializeField] float CameraNormalFOV = 60f;
        [SerializeField] float CameraSprintFOV = 80f;

        float TargetCameraFOV
        {
            get
            {
                return Sprinting ? CameraSprintFOV : CameraNormalFOV;
            }
        }
        #endregion

        #region Physics
        [Header("Physics Parameters")]
        // Used by the kinematic physics equations for jumping.
        private const float JUMP_VELOCITY_FACTOR = -2f;
        
        [SerializeField] float GravityScale = 3f;
        public Vector3 CurrentVelocity { get; private set; }
        public float VerticalVelocity = 0f;
        public float CurrentSpeed { get; private set; }
        public bool IsGrounded => characterController.isGrounded;
        #endregion

        #region Input
        [Header("Input")]
        public Vector2 MoveInput;
        public Vector2 LookInput;
        public bool SprintInput;
        #endregion

        #region Components
        [Header("Components")]
        [SerializeField] CinemachineCamera fpCamera;
        [SerializeField] CharacterController characterController;
        #endregion

        #region Unity Methods
        private void OnValidate()
        {
            if (characterController == null )
            {
                characterController = GetComponent<CharacterController>();
            }
        }

        private void Update()
        {
            MoveUpdate();
            LookUpdate();
            CameraUpdate();
        }

        #endregion

        #region Controller Methods
        public void InitiateJump()
        {
            // Don't modify VerticalVelocity if we're on the ground.
            if (!IsGrounded) return;
            VerticalVelocity = Mathf.Sqrt(JumpHeight * JUMP_VELOCITY_FACTOR  * Physics.gravity.y * GravityScale);
        }
        
        void MoveUpdate()
        {
            Vector3 motion = transform.forward * MoveInput.y + transform.right * MoveInput.x;
            motion.y = 0f;
            motion.Normalize();

            if (motion.sqrMagnitude >= 0.1f)
            {
                CurrentVelocity = Vector3.MoveTowards(CurrentVelocity, motion * MaxSpeed, Acceleration * Time.deltaTime);
            }
            else
            {
                CurrentVelocity = Vector3.MoveTowards(CurrentVelocity, Vector3.zero, Acceleration * Time.deltaTime);
            }

            if (IsGrounded && VerticalVelocity <= 0.01f)
            {
                VerticalVelocity = -3f;
            }
            else
            {
                VerticalVelocity += GravityScale * Time.deltaTime * Physics.gravity.y;
            }
            
            Vector3 fullVelocity = new Vector3(CurrentVelocity.x, VerticalVelocity, CurrentVelocity.z);

            characterController.Move(fullVelocity * Time.deltaTime);
            CurrentSpeed = CurrentVelocity.magnitude;
        }
        void LookUpdate()
        {
            Vector2 input = new Vector2(LookInput.x * LookSensitivity.x, LookInput.y * LookSensitivity.y);
            CurrentPitch -= input.y;
            fpCamera.transform.localRotation = Quaternion.Euler(CurrentPitch, 0f, 0f);
            transform.Rotate(Vector3.up * input.x);
        }

        void CameraUpdate()
        {
            float targetFOV = CameraNormalFOV;
            if (Sprinting)
            {
                float speedRatio = CurrentSpeed / SprintSpeed;
                targetFOV = Mathf.Lerp(CameraNormalFOV, CameraSprintFOV, speedRatio);
            }
            fpCamera.Lens.FieldOfView = Mathf.Lerp(fpCamera.Lens.FieldOfView, targetFOV, CameraFOVSmoothing * Time.deltaTime);
        }
        #endregion
    }

}
