using System.Collections;
using System.Collections.Generic;
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
    [HideInInspector] public CharacterCombatManager characterCombatManager;

    [Header("CHARACTER GROUP")]
    public CharacterGroup characterGroup;

    [Header("STATUS")]
    public NetworkVariable<bool> isDead = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("FLAGS")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;
    public bool isGrounded = true;

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
        characterCombatManager = GetComponent<CharacterCombatManager>();
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

    protected virtual void Start()
    {
        IgnoreOwnColliders();
    }

    protected virtual void LateUpdate()
    {

    }

    protected virtual void FixedUpdate()
    {
        
    }

    // NETWORK
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        anim.SetBool("IsMoving", characterNetworkManager.isMoving.Value);
        characterNetworkManager.OnIsActiveChanged(false, characterNetworkManager.isActive.Value);
        characterNetworkManager.isActive.OnValueChanged += characterNetworkManager.OnIsActiveChanged;

        characterNetworkManager.isMoving.OnValueChanged += characterNetworkManager.OnIsMovingChanged;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        characterNetworkManager.isActive.OnValueChanged -= characterNetworkManager.OnIsActiveChanged;
        characterNetworkManager.isMoving.OnValueChanged -= characterNetworkManager.OnIsMovingChanged;
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

    // DAMAGEBLE COLLIDERS
    protected virtual void IgnoreOwnColliders()
    {
        Collider charactercontrollerCollider = GetComponent<Collider>();
        Collider[] damagableColliders = GetComponentsInChildren<Collider>();
        List<Collider> ignoredColliders = new();

        foreach (var collider in damagableColliders)
        {
            ignoredColliders.Add(collider);
        }

        ignoredColliders.Add(charactercontrollerCollider);

        foreach (var collider in ignoredColliders)
        {
            foreach (var otherCollider in ignoredColliders)
            {
                Physics.IgnoreCollision(collider, otherCollider, true);
            }
        }
    }
}
