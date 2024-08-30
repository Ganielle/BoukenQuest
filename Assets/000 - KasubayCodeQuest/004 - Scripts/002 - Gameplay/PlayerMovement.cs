using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GamePlayerController playerControls;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform cameraTransform;  // Reference to the camera

    [Header("PLAYER MOVEMENTS")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpStrength;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float rotationSmoothTime = 0.1f;

    [Header("ANIMATOR")]
    [SerializeField] private Animator playerAnimator;

    [Header("DEBUGGER")]
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private float targetAngle;
    [SerializeField] private float angle;
    [SerializeField] private float cameraangle;
    [SerializeField] private float currentVelocity;
    [SerializeField] private float velocity;

    private void OnEnable()
    {
        // Initialize any necessary components here
    }

    private void Update()
    {
        Jump();
        ApplyGravity();
        MovePlayer();
        MovementAnimation();
    }

    private void MovePlayer()
    {
        // Get input direction from player controls
        Vector2 input = playerControls.MovementDirection;
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Calculate the target angle based on camera orientation
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, rotationSmoothTime);

            // Rotate the player towards the target angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Move the player in the direction relative to the camera
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        // Apply gravity
        moveDirection.y = velocity;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void Jump()
    {
        if (!playerControls.IsJump) return;
        if (!IsGrounded())
        {
            playerControls.JumpForceStop();
            return;
        }

        velocity += jumpStrength;
    }

    private void ApplyGravity()
    {
        if (IsGrounded() && velocity < 0.0f)
        {
            velocity = -1.0f;
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }

        moveDirection.y = velocity;
    }

    private void MovementAnimation()
    {
        if (playerControls.MovementDirection != Vector2.zero)
            playerAnimator.SetFloat("movement", 1);
        else
            playerAnimator.SetFloat("movement", -1);

        playerAnimator.SetFloat("velocity", velocity);
    }

    private bool IsGrounded() => characterController.isGrounded;
}
