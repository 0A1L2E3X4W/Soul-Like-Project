using Unity.Netcode;
using UnityEngine;

public class CharacterCombatManager : NetworkBehaviour
{
    [Header("MANAGER")]
    protected CharacterManager character;

    [Header("ATK TARGET")]
    public CharacterManager currentTarget;

    [Header("ATK TYPE")]
    public AtkType currentAtkType;

    [Header("LOCK ON")]
    public Transform lockOnTransform;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void SetTarget(CharacterManager newTarget)
    {
        if (character.IsOwner)
        {
            if (newTarget != null)
            {
                currentTarget = newTarget;
                character.characterNetworkManager.currentTargetNetworkID.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
            }
            else
            {
                currentTarget = null;
            }
        }
    }
}
