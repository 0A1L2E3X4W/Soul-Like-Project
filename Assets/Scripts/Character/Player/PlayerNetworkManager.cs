using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkManager : CharacterNetworkManager
{
    [Header("MANAGER")]
    private PlayerManager player;

    [Header("CHARACTER INFO")]
    public NetworkVariable<FixedString64Bytes> characterName =
            new("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }
}
