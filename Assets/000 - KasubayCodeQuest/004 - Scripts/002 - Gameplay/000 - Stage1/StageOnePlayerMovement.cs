using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageOnePlayerMovement : MonoBehaviour
{
    [SerializeField] private StageOneGamePlayerController gamePlayerController;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CinemachineCameraOffset cmOffset;

    [Header("PLAYER MOVEMENTS")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float rotationSmoothTime = 0.1f;
    [SerializeField] private float jumpStrength;
    [SerializeField] private LayerMask collapsePlatformMask;

    [Header("CAMERA")]
    [SerializeField] private Vector3 leftOffset;
    [SerializeField] private Vector3 rightOffset;

    [Header("ANIMATOR")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Vector3 playermovement;
    [SerializeField] private Vector3 playerRotation;
    [SerializeField] private float velocity;

    [Header("DEBUGGER")]
    [SerializeField] private GameObject lastCollidedPlatform = null;

    RaycastHit hit;

    private void OnEnable()
    {
        playerRotation = new Vector3(0f, 90f, 0f);
        cmOffset.m_Offset = Vector3.Lerp(cmOffset.m_Offset, rightOffset, 1f);
    }

    private void Update()
    {
        Jump();
        ApplyGravity();
        MovePlayer();
        CameraOffsets();
        MovementAnimation();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        CollapsingPlatform collapsingPlatform = hit.gameObject.GetComponent<CollapsingPlatform>();

        if (collapsingPlatform != null && hit.gameObject != lastCollidedPlatform)
        {
            collapsingPlatform.StartToCollapse();
            lastCollidedPlatform = hit.gameObject;
        }
        else if (collapsingPlatform == null)
        {
            if (lastCollidedPlatform != null)
            {
                lastCollidedPlatform.GetComponent<CollapsingPlatform>().StopCollapseOnExit();
                lastCollidedPlatform = null;
            }
        }
    }

    private void MovePlayer()
    {
        if (gamePlayerController.IsLeft)
        {
            playermovement = new Vector3(-1 * moveSpeed, 0f, 0f);
            playerRotation = new Vector3(0f, -90f, 0f);
            playerAnimator.SetFloat("movement", 1);
        }
        else if (gamePlayerController.IsRight)
        {
            playermovement = new Vector3(1 * moveSpeed, 0f, 0f);
            playerRotation = new Vector3(0f, 90f, 0f);
            playerAnimator.SetFloat("movement", 1);
        }
        else
        {
            playermovement = Vector3.zero;
            playerAnimator.SetFloat("movement", -1);
        }

        playermovement.y = velocity;

        playerAnimator.transform.rotation = Quaternion.Euler(playerRotation);
        characterController.Move(playermovement * Time.deltaTime);
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

        playermovement.y = velocity;
    }

    private void Jump()
    {
        if (!gamePlayerController.IsJump) return;
        if (!IsGrounded())
        {
            gamePlayerController.JumpForceStop();
            return;
        }

        velocity += jumpStrength;
    }

    private void CameraOffsets()
    {
        if (playerAnimator.transform.rotation == Quaternion.Euler(new Vector3(0f, -90f, 0f)))
        {
            cmOffset.m_Offset = Vector3.Lerp(cmOffset.m_Offset, leftOffset, 0.05f);
        }
        else
        {
            cmOffset.m_Offset = Vector3.Lerp(cmOffset.m_Offset, rightOffset, 0.05f);
        }
    }

    private void MovementAnimation()
    {
        if (gamePlayerController.IsLeft)
        {
            playerAnimator.SetFloat("movement", 1);
        }
        else if (gamePlayerController.IsRight)
        {
            playerAnimator.SetFloat("movement", 1);
        }
        else
        {
            playerAnimator.SetFloat("movement", -1);
        }

        playerAnimator.SetFloat("velocity", velocity);
    }

    private bool IsGrounded() => characterController.isGrounded;
}
