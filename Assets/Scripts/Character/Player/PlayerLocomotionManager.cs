using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    [Header("MANAGER")]
    private PlayerManager player;

    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;

    [Header("MOVEMENT SETTINGS")]
    [SerializeField] private float walkingSpeed = 1.5f;
    [SerializeField] private float runningSpeed = 3.5f;
    [SerializeField] private float rotationSpeed = 15f;
    private Vector3 moveDirection;
    private Vector3 targetRotateDirection;

    [Header("DODGE SETTINGS")]
    [SerializeField] private float dodgeStaminaCost = 25f;
    private Vector3 rollDirection;

    [Header("SPRINT SETTINGS")]
    [SerializeField] private float sprintSpeed = 6.5f;
    [SerializeField] private float sprintStaminaCost = 2f;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (player.IsOwner)
        {
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.moveAmount.Value = moveAmount;
        }
        else
        {
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;

            player.playerAnimatorManager.UpdateAnimatorMovementParams(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
        }
    }

    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotation();
    }

    // GROUND MOVEMENT
    private void GetMovementParams()
    {
        verticalMovement = PlayerInputManager.Instance.verticalInput;
        horizontalMovement = PlayerInputManager.Instance.horizontalInput;
        moveAmount = PlayerInputManager.Instance.moveAmount;
    }

    private void HandleGroundedMovement()
    {
        if (player.canMove || player.canRotate)
        {
            GetMovementParams();
        }

        if (!player.canMove)
            return;

        moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
        moveDirection += PlayerCamera.Instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0f;

        if (player.playerNetworkManager.isSprinting.Value)
        {
            player.characterController.Move(sprintSpeed * Time.deltaTime * moveDirection);
        }
        else
        {
            if (PlayerInputManager.Instance.moveAmount > 0.5f)
            {
                player.characterController.Move(runningSpeed * Time.deltaTime * moveDirection);
            }
            else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
            {
                player.characterController.Move(walkingSpeed * Time.deltaTime * moveDirection);
            }
        }
    }

    private void HandleRotation()
    {
        if (!player.canRotate)
            return;
        
        targetRotateDirection = Vector3.zero;
        targetRotateDirection = PlayerCamera.Instance.cameraObj.transform.forward * verticalMovement;
        targetRotateDirection += PlayerCamera.Instance.cameraObj.transform.right * horizontalMovement;
        targetRotateDirection.Normalize();
        targetRotateDirection.y = 0f;

        if (targetRotateDirection == Vector3.zero)
        {
            targetRotateDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotateDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

    // ACTIONS
    public void AttemptPerformDodge()
    {
        if (player.isPerformingAction)
            return;

        if (player.playerNetworkManager.currentStamina.Value <= 0f)
            return;

        if (PlayerInputManager.Instance.moveAmount > 0)
        {
            rollDirection = PlayerCamera.Instance.cameraObj.transform.forward * PlayerInputManager.Instance.verticalInput;
            rollDirection += PlayerCamera.Instance.cameraObj.transform.right * PlayerInputManager.Instance.horizontalInput;
            rollDirection.y = 0f;
            rollDirection.Normalize();

            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;

            player.playerAnimatorManager.PlayTargetActionAnim("Roll_Forward_01", true, true);
        }
        else
        {
            player.playerAnimatorManager.PlayTargetActionAnim("Back_Step_01", true, true);
        }

        player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
    }

    public void HandleSprinting()
    {
        if (player.isPerformingAction)
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }

        if (player.playerNetworkManager.currentStamina.Value <= 0f)
        {
            player.playerNetworkManager.isSprinting.Value = false;
            return;
        }

        if (moveAmount >= 0.5f)
        {
            player.playerNetworkManager.isSprinting.Value = true;
        }
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }

        if (player.playerNetworkManager.isSprinting.Value)
        {
            player.playerNetworkManager.currentStamina.Value -= sprintStaminaCost * Time.deltaTime;
        }
    }
}
