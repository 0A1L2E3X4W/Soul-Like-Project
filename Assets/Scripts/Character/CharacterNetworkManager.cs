using Unity.Netcode;
using UnityEngine;

public class CharacterNetworkManager : NetworkBehaviour
{
    [Header("MANAGER")]
    private CharacterManager character;

    [Header("POSITION & ROTATION")]
    public Vector3 networkPositionVelocity;
    public float networkPostionSmoothTime = 0.1f;
    public float networkRotationSmoothTime = 0.1f;
    public NetworkVariable<Vector3> networkPosition =
        new(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> networkRotation =
        new(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("ANIMATION")]
    public NetworkVariable<float> horizontalMovement =
        new(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> verticalMovement =
        new(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> moveAmount =
        new(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("FLAGS")]
    public NetworkVariable<bool> isSprinting = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isJumping = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isLockedOn = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isChargingAtk = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isMoving = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("STATS")]
    public NetworkVariable<int> endurance =
        new(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> vitality =
            new(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("RESOURCES")]
    public NetworkVariable<float> currentStamina =
        new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> maxStamina =
        new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentHealth =
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> maxHealth =
        new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("TARGET")]
    public NetworkVariable<ulong> currentTargetNetworkID =
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    // IS MOVING
    public void OnIsMovingChanged(bool oldState, bool newState)
    {
        character.anim.SetBool("IsMoving", isMoving.Value);
    }

    // STATS
    public virtual void CheckHp(int oldVal, int newVal)
    {
        if (currentHealth.Value <= 0)
        {
            StartCoroutine(character.ProcessDeathEvent());
        }

        if (character.IsOwner)
        {
            if (currentHealth.Value > maxHealth.Value)
            {
                currentHealth.Value = maxHealth.Value;
            }
        }
    }

    // TARGET
    public void OnTargetIDChanged(ulong oldID, ulong newID)
    {
        if (!IsOwner)
        {
            character.characterCombatManager.currentTarget =
                NetworkManager.Singleton.SpawnManager.SpawnedObjects[newID].gameObject.GetComponent<CharacterManager>();
        }
    }

    // LOCK ON
    public void OnIsLockedOnChanged(bool oldVal, bool isLockedOn)
    {
        if (!isLockedOn)
        {
            character.characterCombatManager.currentTarget = null;
        }
    }

    // CHARGED ATTACK
    public void OnIsChargingAtkChanged(bool oldStatus, bool newStatus)
    {
        character.anim.SetBool("IsChargingAtk", isChargingAtk.Value);
    }

    // PLAY TARGET ANIMATIONS
    [ServerRpc]
    public void NotifyServerOfActionAnimServerRpc(ulong clientID, string animID, bool applyRootMotion)
    {
        if (IsServer)
        {
            PlayActionAnimForAllClientClientRpc(clientID, animID, applyRootMotion);
        }
    }

    [ClientRpc]
    public void PlayActionAnimForAllClientClientRpc(ulong clientID, string animID, bool applyRootMotion)
    {
        if (clientID != NetworkManager.Singleton.LocalClientId)
        {
            PerformActionAnimFromServer(animID, applyRootMotion);
        }
    }

    private void PerformActionAnimFromServer(string animID, bool applyRootMotion)
    {
        character.applyRootMotion = applyRootMotion;
        character.anim.CrossFade(animID, 0.2f);
    }

    // PLAY ATTACK ACTION ANIMATIONS
    [ServerRpc]
    public void NotifyServerOfAtkActionAnimServerRpc(ulong clientID, string animID, bool applyRootMotion)
    {
        if (IsServer)
        {
            PlayAtkActionAnimForAllClientClientRpc(clientID, animID, applyRootMotion);
        }
    }

    [ClientRpc]
    public void PlayAtkActionAnimForAllClientClientRpc(ulong clientID, string animID, bool applyRootMotion)
    {
        if (clientID != NetworkManager.Singleton.LocalClientId)
        {
            PerformAtkActionAnimFromServer(animID, applyRootMotion);
        }
    }

    private void PerformAtkActionAnimFromServer(string animID, bool applyRootMotion)
    {
        character.applyRootMotion = applyRootMotion;
        character.anim.CrossFade(animID, 0.2f);
    }

    // DAMAGE
    [ServerRpc(RequireOwnership = false)]
    public void NotifyServerOfCharacterDamageServerRpc(ulong damagedCharacterID, ulong characterCausingDamageID,
        float physicalDamage, float magicDamage, float holyDamage, float fireDamage, float lightningDamage, float poiseDamage,
        float angleHitFrom, float contactPointX, float contactPointY, float contackPointZ)
    {
        if (IsServer)
        {
            NotifyServerOfCharacterDamageClientRpc(damagedCharacterID, characterCausingDamageID,
                physicalDamage, magicDamage, holyDamage, fireDamage, lightningDamage, poiseDamage,
                angleHitFrom, contactPointX, contactPointY, contackPointZ);
        }
    }

    [ClientRpc]
    private void NotifyServerOfCharacterDamageClientRpc(ulong damagedCharacterID, ulong characterCausingDamageID,
        float physicalDamage, float magicDamage, float holyDamage, float fireDamage, float lightningDamage, float poiseDamage,
        float angleHitFrom, float contactPointX, float contactPointY, float contackPointZ)
    {
        ProcessDamageFromServer(damagedCharacterID, characterCausingDamageID,
                physicalDamage, magicDamage, holyDamage, fireDamage, lightningDamage, poiseDamage,
                angleHitFrom, contactPointX, contactPointY, contackPointZ);
    }

    private void ProcessDamageFromServer(ulong damagedCharacterID, ulong characterCausingDamageID,
        float physicalDamage, float magicDamage, float holyDamage, float fireDamage, float lightningDamage, float poiseDamage,
        float angleHitFrom, float contactPointX, float contactPointY, float contackPointZ)
    {
        CharacterManager damagedCharacter =
            NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].gameObject.GetComponent<CharacterManager>();
        CharacterManager characterCausingDamage =
            NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.GetComponent<CharacterManager>();

        TakeDamage damageEffect = Instantiate(WorldEffectsManager.Instance.takeDamageEffect);

        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.lightningDamage = lightningDamage;
        damageEffect.poiseDamage = poiseDamage;
        damageEffect.angleHitFrom = angleHitFrom;
        damageEffect.contactPoint = new Vector3(contactPointX, contactPointY, contackPointZ);
        damageEffect.characterCausingDamage = characterCausingDamage;

        damagedCharacter.characterEffectsManager.ProcessInstanceEffect(damageEffect);
    }
}
