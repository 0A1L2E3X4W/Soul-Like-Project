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

    [Header("FLAGS")]
    public bool canComboOnMainHand = false;
    public bool canPerformRollAtk = false;
    public bool canPerformBackStepAtk = false;

    [Header("LAST PERFORMED ACTION ANIM")]
    public string lastAtkAnimPerformed;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    // TARGET
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

    // COMBO
    public virtual void EnableCombo()
    {

    }

    public virtual void DisableCombo()
    {

    }

    // INVULNERABLE
    public void EnableIsInvulenerable()
    {
        if (character.IsOwner)
            character.characterNetworkManager.isInvulenerable.Value = true;
    }

    public void DisableIsInvulenerable()
    {
        if (character.IsOwner)
            character.characterNetworkManager.isInvulenerable.Value = false;
    }
}
