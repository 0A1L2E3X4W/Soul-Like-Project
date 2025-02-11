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

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

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
}
