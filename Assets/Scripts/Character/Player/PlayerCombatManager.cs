using Unity.Netcode;
using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{
    [Header("MANAGER")]
    private PlayerManager player;

    [Header("CURRENT WEAPON")]
    public WeaponItem currentWeaponBeingUsed;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
    {
        if (player.IsOwner)
        {
            weaponAction.AttemptPerformAction(player, weaponPerformingAction);

            player.playerNetworkManager.NotifyServerOfWeaponActionServerRpc(
                NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformingAction.itemID);
        }
    }

    public virtual void DrainStaminaBasedAtk()
    {
        if (!player.IsOwner)
            return;

        if (currentWeaponBeingUsed == null)
            return;

        float staminaDeducted = 0f;

        switch (currentAtkType)
        {
            case AtkType.LightAtk01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAtkStaminaCostMultiplier;
                break;
            case AtkType.LightAtk02:
                break;
            case AtkType.HeavyAtk01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAtkStaminaCostMultiplier;
                break;
            case AtkType.ChargedAtk01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAtkStaminaCostMultiplier;
                break;
            default:
                break;
        }

        Debug.Log("STAMINA COST: " + staminaDeducted);
        player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);
    }

    public override void SetTarget(CharacterManager newTarget)
    {
        base.SetTarget(newTarget);

        if (player.IsOwner)
        {
            PlayerCamera.Instance.SetLockedOnCameraHeight();
        }
    }
}
