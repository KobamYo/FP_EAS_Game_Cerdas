using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    // Declares reference variables
    PlayerInput playerInput; // Must be generated first from New Input System
    CharacterController characterController;
    Animator animator;

    // Variables to store optimized setter/getter parameter IDs
    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    int jumpCountHash;

    // Variables to store player input values
    Vector3 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement; 
    Vector3 appliedMovement;
    bool isMovementPressed;
    bool isRunPressed;

    // Constants
    float rotationFactorPerFrame = 20.0f;
    float runMultiplier = 5.0f;

    // Gravity variables
    float gravity = -9.8f;
    float groundedGravity = -.05f;

    // Jumping variables
    bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 2.0f;
    float maxJumpTime = 0.75f;
    bool isJumping = false;
    bool requireNewJumpPress = false;
    bool isJumpAnimating = false;
    int jumpCount = 0;
    Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> jumpGravities = new Dictionary<int, float>();
    Coroutine currentJumpResetRoutine = null;

    // State Variables
    PlayerBaseState currentState;
    PlayerStateFactory states;

    // Getter and setter
    public PlayerBaseState CurrentState { get { return currentState; } set { currentState = value; }}
    public Animator Animator { get { return animator; }}
    public CharacterController CharacterController { get { return characterController; }}
    public Coroutine CurrentJumpResetRoutine { get { return currentJumpResetRoutine; } set { currentJumpResetRoutine = value; }}
    public Dictionary<int, float> InitialJumpVelocities { get { return initialJumpVelocities; }}
    public Dictionary<int, float> JumpGravities { get { return jumpGravities; }}
    public int JumpCount { get { return jumpCount; } set { jumpCount = value; }}
    public int IsWalkingHash {get { return isWalkingHash; }}
    public int IsRunningHash {get { return isRunningHash; }}
    public int IsJumpingHash { get { return isJumpingHash; }}
    public int JumpCountHash { get { return jumpCountHash; }}
    public bool IsMovementPressed {get {return isMovementPressed; }}
    public bool IsRunPressed { get {return isRunPressed; }}
    public bool RequireNewJumpPress { get { return requireNewJumpPress; } set { requireNewJumpPress = value; }}
    public bool IsJumping { set { isJumping = value; }}
    public bool IsJumpPressed { get { return isJumpPressed; }}
    public float GroundedGravity { get { return groundedGravity; }}
    public float CurrentMovementY { get { return currentMovement.y; } set { currentMovement.y = value; }}
    public float AppliedMovementY { get { return appliedMovement.y; } set { appliedMovement.y = value; }}
    public float AppliedMovementX { get { return appliedMovement.x; } set { appliedMovement.x = value; }}
    public float AppliedMovementZ { get { return appliedMovement.z; } set { appliedMovement.z = value; }}
    public float RunMultiplier { get { return runMultiplier; }}
    public Vector3 CurrentMovementInput { get { return currentMovementInput; }}

    // Awake is called earlier than Start in Unity's event life cycle
    void Awake()
    {
        // Initially set reference variables
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Setup states
        states = new PlayerStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();

        // Set the parameter hash references
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        jumpCountHash = Animator.StringToHash("jumpCount");

        // Set the player input callbacks
        playerInput.CharacterControls.Move.started += OnMovementInput; // Callback input when started
        playerInput.CharacterControls.Move.canceled += OnMovementInput; // Cancelling input
        playerInput.CharacterControls.Move.performed += OnMovementInput; // Callback for gamepad
        playerInput.CharacterControls.Run.started += OnRun;
        playerInput.CharacterControls.Run.canceled += OnRun;
        playerInput.CharacterControls.Jump.started += OnJump;
        playerInput.CharacterControls.Jump.canceled += OnJump;
        
        SetupJumpVariables();
    }

    // Set the initial velocity and gravity using jump heights and durations
    void SetupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        float secondJumpGravity = (-2 * (maxJumpHeight + 2)) / Mathf.Pow((timeToApex * 1.25f), 2);
        float secondInitialJumpVelocity = (2 * (maxJumpHeight + 2)) / (timeToApex * 1.25f);
        float thirdJumpGravity = (-2 * (maxJumpHeight + 4)) / Mathf.Pow((timeToApex * 1.5f), 2);
        float thirdInitialJumpVelocity = (2 * (maxJumpHeight + 4)) / (timeToApex * 1.5f);

        initialJumpVelocities.Add(1, initialJumpVelocity);
        initialJumpVelocities.Add(2, secondInitialJumpVelocity);
        initialJumpVelocities.Add(3, thirdInitialJumpVelocity);

        jumpGravities.Add(0, gravity);
        jumpGravities.Add(1, gravity);
        jumpGravities.Add(2, secondJumpGravity);
        jumpGravities.Add(3, thirdJumpGravity);
    }

    void Update()
    {
        HandleRotation();
        currentState.UpdateState();
        characterController.Move(appliedMovement * Time.deltaTime);
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;

        // The change of position our character should point to
        positionToLookAt.x = currentMovementInput.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovementInput.z;
        
        // The current rotation of our character
        Quaternion currentRotation = transform.rotation;

        if(isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    // Callback handler to set the player input values
    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        // currentMovement.x = currentMovementInput.x;
        // currentMovement.z = currentMovementInput.y;
        // currentRunMovement.x = currentMovementInput.x * runMultiplier;
        // currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    // Callback handler for jump buttons
    void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
        requireNewJumpPress = false;
    }

    // Callback handler for run buttons
    void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void OnEnable()
    {
        // Enable character controls action map
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        // Disable character controls action map
        playerInput.CharacterControls.Disable();
    }
}
