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

    [Header("STATS")]
    public NetworkVariable<int> endurance =
        new(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> vitality =
            new(10, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

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

    #region PLAY TARGET ANIMATIONS
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
    #endregion
}
