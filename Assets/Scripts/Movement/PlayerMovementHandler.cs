using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    [HideInInspector] public CharacterController characterController;
    PlayerAnimationHandler playerAnimationHandler;
    WeaponInventory weaponInventory;
    InputHandler inputHandler;
    Transform cam;

    [Header("Movement")]
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float crouchSpeed = 2f;
    [SerializeField] float airSpeed = 0.5f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float normalHeight = 2f;
    [SerializeField] float crouchHeight = 1f;
    float speed;
    float refSpeed;
    bool isJumping;
    bool isCrouched;

    [Header("Gravity")]
    [SerializeField] float gravity = -9.81f;
    Vector3 verticalVelocity = Vector3.zero;
    [SerializeField] LayerMask groundMask;
    [HideInInspector] public bool isGrounded;

    [Header("Camera")]
    [SerializeField] Transform playerCameraHandler;
    [SerializeField] float xClamp = 85f;
    [SerializeField] float sensitivityX = 8f;
    [SerializeField] float sensitivityY = 0.5f;
    float xRotation = 0f;

    [Header("Extras")]
    [SerializeField] Transform armatureHead;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        weaponInventory = GetComponent<WeaponInventory>();
        inputHandler = GetComponent<InputHandler>();
    }

    void Update()
    {
        HandleJump();
        HandleSprintAndCrouch();
        HandleMovement();
    }

    void LateUpdate()
    {
        HandleRotation();
    }

    void HandleJump()
    {
        isJumping = inputHandler.jumpInput;
    }

    void HandleSprintAndCrouch()
    {
        playerCameraHandler.position = armatureHead.position;
        if (inputHandler.crouchInput)
        {
            characterController.height = crouchHeight;
            characterController.center = new Vector3(0f, 0.58f, 0f);
            isCrouched = true;
            speed = Mathf.SmoothDamp(speed, crouchSpeed, ref refSpeed, 0.1f);
            playerAnimationHandler.SetBool(playerAnimationHandler.isCrouchingHash, true);
        }
        else
        {
            if (inputHandler.walkInput)
            {
                speed = Mathf.SmoothDamp(speed, sprintSpeed, ref refSpeed, 0.1f);
            }
            else
            {
                speed = Mathf.SmoothDamp(speed, walkSpeed, ref refSpeed, 0.1f);
            }
            isCrouched = false;
            characterController.height = normalHeight;
            characterController.center = new Vector3(0f, 1.08f, 0f);
            playerAnimationHandler.SetBool(playerAnimationHandler.isCrouchingHash, false);
        }
    }

    void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundMask);
        playerAnimationHandler.SetBool(playerAnimationHandler.isGroundedHash, isGrounded);
        if (isGrounded)
        {
            verticalVelocity.y = 0f;
        }
        float newSpeed = speed * weaponInventory.currentWeaponItem.movementSpeedFactor;
        Vector3 movementVelocity = (inputHandler.movementInput.x * transform.right + inputHandler.movementInput.y * transform.forward).normalized * newSpeed;
        movementVelocity.y = 0f;

        if (!isGrounded)
        {
            characterController.Move(movementVelocity * airSpeed * Time.deltaTime);
        }
        else
        {
            characterController.Move(movementVelocity * Time.deltaTime);
        }

        playerAnimationHandler.UpdateAnimatorValues(inputHandler.movementInput.y, inputHandler.movementInput.x, inputHandler.walkInput, inputHandler.crouchInput, weaponInventory.CurrentWeaponAnimator);

        if (isJumping && isGrounded)
        {
            if (isCrouched)
            {
                verticalVelocity.y = Mathf.Sqrt(-2 * (jumpHeight + 0.5f) * gravity);
            }
            else
            {
                verticalVelocity.y = Mathf.Sqrt(-2 * jumpHeight * gravity);
            }
            playerAnimationHandler.PlayTargetAnimation("JumpStart", false);
            inputHandler.jumpInput = false;
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        characterController.Move(verticalVelocity * Time.deltaTime);
    }

    void HandleRotation()
    {
        float mouseX = inputHandler.cameraInput.x * sensitivityX;
        float mouseY = inputHandler.cameraInput.y * sensitivityY;

        transform.Rotate(Vector3.up, mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);
        playerCameraHandler.localEulerAngles = new Vector3(xRotation, 0, 0);
    }
}
