using System;
using UnityEngine;

namespace Player_Scripts
{
    public class PlayerController3D : MonoBehaviour
    {
        //basic first person controller
        //just walks around, no jump/sprint
        //this code was outsourced from previous projects
        [SerializeField] private Transform playerRoot, lookRoot;
        private bool invert;
        private float sensivity = 5f;
        private Vector2 defaultLimits = new Vector2(-70f, 80f);
        private Vector2 lookAngles;
        private Vector2 _currentMouseLook;
        private Vector2 smoothMove;
        private float currentRollAngle;
        private int lastLookFrame;
        private CharacterController characterController;
        private Vector3 moveDirection;
        private float speed = 5f;
        private float gravity = 20f;
        private float verticalVelocity;

        #region Callbacks

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        //the cursor needs to be locked and invisible for the player to look around
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            MoveThePlayer();
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                LookAround();
            }
        }
        #endregion
     
        //Use Unity's basic inputs to move around
        void MoveThePlayer()
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f,
                Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed * Time.deltaTime;

            ApplyGravity();
            characterController.Move(moveDirection);
        }

        void ApplyGravity()
        {
            verticalVelocity -= gravity * Time.deltaTime;
            moveDirection.y = verticalVelocity * Time.deltaTime;
        }
        
        //use the mouse to look around
        private void LookAround() 
        {
            _currentMouseLook = new Vector2(
                Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

            lookAngles.x += _currentMouseLook.x * sensivity * (invert ? 1f : -1f);
            lookAngles.y += _currentMouseLook.y * sensivity;
            lookAngles.x = Mathf.Clamp(lookAngles.x, defaultLimits.x, defaultLimits.y);

            lookRoot.localRotation = Quaternion.Euler(lookAngles.x, 0f, 0f);
            playerRoot.localRotation = Quaternion.Euler(0f, lookAngles.y, 0f);
        }
    }
}
