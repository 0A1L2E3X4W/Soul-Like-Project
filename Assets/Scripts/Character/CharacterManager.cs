using Unity.Netcode;
using UnityEngine;

public class CharacterManager : NetworkBehaviour
{
    [Header("CHARACTER ESSENTICALS")]
    [HideInInspector] public Animator anim;
    [HideInInspector] public CharacterController characterController;

    [Header("MANAGERS")]
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector] public CharacterNetworkManager characterNetworkManager;
    [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;
    [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;
    [HideInInspector] public CharacterStatsManager characterStatsManager;

    [Header("FLAGS")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;
    public bool isGrounded = true;
    public bool isJumping = false;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);

        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
        characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
    }

    protected virtual void Update()
    {
        anim.SetBool("IsGrounded", isGrounded);

        if (IsOwner)
        {
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position,
                characterNetworkManager.networkPosition.Value,
                ref characterNetworkManager.networkPositionVelocity,
                characterNetworkManager.networkPostionSmoothTime);

            transform.rotation = Quaternion.Slerp(transform.rotation,
                characterNetworkManager.networkRotation.Value,
                characterNetworkManager.networkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate()
    {

    }
}
