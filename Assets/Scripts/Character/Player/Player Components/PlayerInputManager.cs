using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;

    [Header("MANAGER")]
    public PlayerManager player;

    [Header("INPUT SYSTEM")]
    private PlayerControls playerControls;

    [Header("PLAYER MOVEMENT INPUT")]
    [SerializeField] private Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("PLAYER CAMERA INPUT")]
    [SerializeField] private Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    [Header("PLAYER ACTION INPUT")]
    [SerializeField] private bool dodgeInput = false;
    [SerializeField] private bool sprintInput = false;
    [SerializeField] private bool jumpInput = false;

    [Header("SWITCH ITEMS / WEAPONS SLOTS")]
    [SerializeField] private bool switchRightSlotInput = false;
    [SerializeField] private bool switchLeftSlotInput = false;

    [Header("BUMPER INPUTS")]
    [SerializeField] private bool rbInput = false;

    [Header("TRIGGER INPUTS")]
    [SerializeField] private bool rtInput = false;
    [SerializeField] private bool holdRTInput = false;

    [Header("LOCK ON INPUT")]
    [SerializeField] private bool lockOnInput = false;
    [SerializeField] private bool lockOnLeftInput = false;
    [SerializeField] private bool lockOnRightInput = false;
    private Coroutine lockOnCoroutine;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.activeSceneChanged += OnSceneChange;
        Instance.enabled = false;

        if (playerControls != null)
        {
            playerControls.Disable();
        }
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        if (newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex())
        {
            Instance.enabled = true;

            if (playerControls != null)
            {
                playerControls.Enable();
            }
        }
        else
        {
            Instance.enabled = false;

            if (playerControls != null)
            {
                playerControls.Disable();
            }
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();

            // TAP ACTION
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
            playerControls.PlayerActions.SwitchRightWeapon.performed += i => switchRightSlotInput = true;
            playerControls.PlayerActions.SwitchLeftWeapon.performed += i => switchLeftSlotInput = true;

            // HOLD ACTION
            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

            // RIGHT BUMPER
            playerControls.PlayerActions.RB.performed += i => rbInput = true;

            // RIGHT TRIGGER
            playerControls.PlayerActions.RT.performed += i => rtInput = true;
            playerControls.PlayerActions.HoldRT.performed += i => holdRTInput = true;
            playerControls.PlayerActions.HoldRT.canceled += i => holdRTInput = false;

            // LOCK ON
            playerControls.PlayerActions.LockOn.performed += i => lockOnInput = true;
            playerControls.PlayerActions.SeekRightTarget.performed += i => lockOnRightInput = true;
            playerControls.PlayerActions.SeekLeftTarget.performed += i => lockOnLeftInput = true;
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus) { playerControls.Enable(); }
            else { playerControls.Disable(); }
        }
    }

    private void Update()
    {
        HandleAllInputs();
    }

    private void HandleAllInputs()
    {
        HandleLockOnInput();
        HandleSwitchLockOnTargetInput();

        HandleMovementInput();
        HandleCameraInput();

        HandleDodgeInput();
        HandleSprintInput();
        HandleJumpInput();

        HandleRbInput();
        HandleRtInput();
        HandleHoldRtInput();

        HandleSwitchRightSlotInput();
        HandleSwitchLeftSlotInput();
    }

    // LOCK ON
    private void HandleLockOnInput()
    {
        if (player.playerNetworkManager.isLockedOn.Value)
        {
            if (player.playerCombatManager.currentTarget == null)
                return;

            if (player.playerCombatManager.currentTarget.isDead.Value)
            {
                player.playerNetworkManager.isLockedOn.Value = false;
            }

            if (lockOnCoroutine != null)
                StopCoroutine(lockOnCoroutine);

            lockOnCoroutine = StartCoroutine(PlayerCamera.Instance.WaitFindNewTarget());
        }

        if (lockOnInput && player.playerNetworkManager.isLockedOn.Value)
        {
            lockOnInput = false;
            PlayerCamera.Instance.ClearLockOnTargets();
            player.playerNetworkManager.isLockedOn.Value = false;
            return;
        }

        if (lockOnInput && !player.playerNetworkManager.isLockedOn.Value)
        {
            lockOnInput = false;

            PlayerCamera.Instance.HandleLocateLockOnTarget();

            if (PlayerCamera.Instance.nearestLockOnTarget != null)
            {
                player.playerCombatManager.SetTarget(PlayerCamera.Instance.nearestLockOnTarget);
                player.playerNetworkManager.isLockedOn.Value = true;
            }
        }
    }

    private void HandleSwitchLockOnTargetInput()
    {
        if (lockOnLeftInput)
        {
            lockOnLeftInput = false;

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                PlayerCamera.Instance.HandleLocateLockOnTarget();

                if (PlayerCamera.Instance.leftLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.Instance.leftLockOnTarget);
                }
            }
        }

        if (lockOnRightInput)
        {
            lockOnRightInput = false;

            if (player.playerNetworkManager.isLockedOn.Value)
            {
                PlayerCamera.Instance.HandleLocateLockOnTarget();

                if (PlayerCamera.Instance.rightLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.Instance.rightLockOnTarget);
                }
            }
        }
    }

    // PLAYER MOVEMENT
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        if (moveAmount <= 0.5f && moveAmount > 0) { moveAmount = 0.5f; }
        else if (moveAmount <= 1f && moveAmount > 0.5f) { moveAmount = 1f; }

        if (player == null)
            return;

        if (!player.playerNetworkManager.isLockedOn.Value || player.playerNetworkManager.isSprinting.Value)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParams(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
        }
        else
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParams(horizontalInput, verticalInput, player.playerNetworkManager.isSprinting.Value);
        }
    }

    // CAMERA
    private void HandleCameraInput()
    {
        cameraHorizontalInput = cameraInput.x;
        cameraVerticalInput = cameraInput.y;
    }

    // ACTIONS
    private void HandleDodgeInput()
    {
        if (dodgeInput)
        {
            dodgeInput = false;
            player.playerLocomotionManager.AttemptPerformDodge();
        }
    }

    private void HandleSprintInput()
    {
        if (sprintInput)
        {
            player.playerLocomotionManager.HandleSprinting();
        }
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }
    }

    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput = false;

            player.playerLocomotionManager.AttemptPerformJump();
        }
    }

    // TRIGGER & BUMPER
    private void HandleRbInput()
    {
        if (rbInput)
        {
            rbInput = false;

            player.playerNetworkManager.SetCharacterActionHand(true);

            player.playerCombatManager.PerformWeaponBasedAction(
                player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action,
                player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    private void HandleRtInput()
    {
        if (rtInput)
        {
            rtInput = false;

            player.playerNetworkManager.SetCharacterActionHand(true);

            player.playerCombatManager.PerformWeaponBasedAction(
                player.playerInventoryManager.currentRightHandWeapon.oh_RT_Action,
                player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    private void HandleHoldRtInput()
    {
        if (player.isPerformingAction)
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerNetworkManager.isChargingAtk.Value = holdRTInput;
            }
        }
    }

    // SWITCH SLOT
    private void HandleSwitchRightSlotInput()
    {
        if (switchRightSlotInput)
        {
            switchRightSlotInput = false;
            player.playerEquipmentManager.SwitchRightWeapon();
        }
    }

    private void HandleSwitchLeftSlotInput()
    {
        if (switchLeftSlotInput)
        {
            switchLeftSlotInput = false;
            player.playerEquipmentManager.SwitchLeftWeapon();
        }
    }
}
