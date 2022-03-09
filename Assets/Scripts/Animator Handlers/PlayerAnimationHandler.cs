using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimationHandler : MonoBehaviour
{
    Animator animator;
    PlayerMovementHandler playerMovementHandler;

    public int horizontalHash;
    public int verticalHash;
    public int isInteractingHash;
    public int isGroundedHash;
    public int isCrouchingHash;
    public int isRunningHash;
    public int movementHash;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovementHandler = GetComponent<PlayerMovementHandler>();
        SetAnimatorHashValues();
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isWalking, bool isCrouched, Animator externalAnimator)
    {
        float v = 0;
        float h = 0;

        if (!isWalking || isCrouched)
        {
            if (verticalMovement > 0)
            {
                v = 0.5f;
            }
            else if (verticalMovement < 0)
            {
                v = -0.5f;
            }
            else
            {
                v = 0;
            }

            if (horizontalMovement > 0)
            {
                h = 0.5f;
            }
            else if (horizontalMovement < 0)
            {
                h = -0.5f;
            }
            else
            {
                h = 0;
            }
            externalAnimator.SetBool(isRunningHash, false);
        }
        else
        {
            if (verticalMovement > 0)
            {
                v = 1f;
            }
            else if (verticalMovement < 0)
            {
                v = -1f;
            }
            else
            {
                v = 0;
            }

            if (horizontalMovement > 0)
            {
                h = 1f;
            }
            else if (horizontalMovement < 0)
            {
                h = -1f;
            }
            else
            {
                h = 0;
            }
            externalAnimator.SetBool(isRunningHash, true);
        }

        if (h > 0 || v > 0)
        {
            externalAnimator.SetBool(movementHash, true);
        }
        else
        {
            externalAnimator.SetBool(movementHash, false);
        }

        animator.SetFloat(verticalHash, v, 0.1f, Time.deltaTime);
        animator.SetFloat(horizontalHash, h, 0.1f, Time.deltaTime);
    }

    public void UpdateMovementValues(float movementSpeed, bool isRunning)
    {
        bool isMoving = movementSpeed > 0.1f;
    }

    void SetAnimatorHashValues()
    {
        horizontalHash = Animator.StringToHash("Horizontal");
        verticalHash = Animator.StringToHash("Vertical");
        isInteractingHash = Animator.StringToHash("IsInteracting");
        isGroundedHash = Animator.StringToHash("IsGrounded");
        isCrouchingHash = Animator.StringToHash("IsCrouching");
        isRunningHash = Animator.StringToHash("IsRunning");
        movementHash = Animator.StringToHash("IsMoving");
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting = false, Animator externalAnimator = null)
    {
        if (externalAnimator != null)
        {
            externalAnimator.SetBool(isInteractingHash, isInteracting);
            externalAnimator.CrossFade(targetAnim, 0.1f);
        }
        else
        {
            animator.SetBool(isInteractingHash, isInteracting);
            animator.CrossFade(targetAnim, 0.2f);
        }
    }

    public void SetBool(int targetBoolhash, bool value)
    {
        animator.SetBool(targetBoolhash, value);
    }

    void OnAnimatorMove()
    {
        Vector3 newPosition = animator.deltaPosition;
        newPosition.y = 0;
        Vector3 velocity = newPosition / Time.deltaTime;
        playerMovementHandler.characterController.velocity.Set(velocity.x, velocity.y, velocity.z);
    }
}
