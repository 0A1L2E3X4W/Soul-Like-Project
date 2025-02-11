using System.Collections;
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
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public CharacterEquipmentManager characterEquipmentManager;
    [HideInInspector] public CharacterInventoryManager characterInventoryManager;

    [Header("STATUS")]
    public NetworkVariable<bool> isDead = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

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
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterEquipmentManager = GetComponent<CharacterEquipmentManager>();
        characterInventoryManager = GetComponent<CharacterInventoryManager>();
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

    // DEATH
    public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnim = false)
    {
        if (IsOwner)
        {
            characterNetworkManager.currentHealth.Value = 0;
            isDead.Value = true;

            if (!manuallySelectDeathAnim)
            {
                characterAnimatorManager.PlayTargetActionAnim("Death_01", true);
            }
        }

        yield return new WaitForSeconds(5);
    }

    // RESPAWN CHARACTER
    public virtual void ReviveCharacter()
    {

    }
}
