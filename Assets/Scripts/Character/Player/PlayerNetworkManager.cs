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

    [Header("EQUIPMENT")]
    public NetworkVariable<int> currentWeaponBeingUsed =
            new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentRightHandWeaponID =
        new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentLeftHandWeaponID =
        new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isUsingRightHand =
        new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isUsingLeftHand =
        new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("TWO HAND EQUIPMENT")]
    public NetworkVariable<bool> isTwoHandingWeapon =
        new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isTwoHandingRightWeapon =
        new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isTwoHandingLeftWeapon =
        new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentWeaponBeingTwoHanded =
        new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    // STATS
    public void SetNewMaxHealthVal(int oldVitality, int newVitality)
    {
        maxHealth.Value = player.playerStatsManager.CalcuHealthBasedOnVitalityLV(newVitality);
        PlayerUIManager.Instance.playerUIHudManager.SetMaxHealthVal(maxHealth.Value);
        currentHealth.Value = maxHealth.Value;
    }

    public void SetNewMaxStaminaVal(int oldEndurance, int newEndurance)
    {
        maxStamina.Value = player.playerStatsManager.CalcuStaminaBasedOnEnduranceLV(newEndurance);
        PlayerUIManager.Instance.playerUIHudManager.SetMaxStaminaVal(maxStamina.Value);
        currentStamina.Value = maxStamina.Value;
    }

    // EQUIP
    public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
        player.playerInventoryManager.currentRightHandWeapon = newWeapon;
        player.playerEquipmentManager.LoadRightWeapon();

        if (player.IsOwner)
        {
            PlayerUIManager.Instance.playerUIHudManager.SetRightQuickSlotIcon(newID);
        }
    }

    public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
        player.playerInventoryManager.currentLeftHandWeapon = newWeapon;
        player.playerEquipmentManager.LoadLeftWeapon();

        if (player.IsOwner)
        {
            PlayerUIManager.Instance.playerUIHudManager.SetLeftQuickSlotIcon(newID);
        }
    }

    public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
        player.playerCombatManager.currentWeaponBeingUsed = newWeapon;

        if (player.IsOwner)
            return;

        if (player.playerCombatManager.currentWeaponBeingUsed != null)
            player.playerAnimatorManager.UpdateAnimatorController(player.playerCombatManager.currentWeaponBeingUsed.weaponAnim);
    }

    public void SetCharacterActionHand(bool isRightHandAction)
    {
        if (isRightHandAction)
        {
            isUsingLeftHand.Value = false;
            isUsingRightHand.Value = true;
        }
        else
        {
            isUsingRightHand.Value = false;
            isUsingLeftHand.Value = true;
        }
    }

    // BLOCKING
    public override void OnIsBlockingChanged(bool oldStatus, bool newStatus)
    {
        base.OnIsBlockingChanged(oldStatus, newStatus);

        if (IsOwner)
        {
            player.playerStatsManager.blockingPhysicalAbsorption = player.playerCombatManager.currentWeaponBeingUsed.physicalBaseDamageAbsorption;
            player.playerStatsManager.blockingMagicAbsorption = player.playerCombatManager.currentWeaponBeingUsed.magicBaseDamageAbsorption;
            player.playerStatsManager.blockingFireAbsorption = player.playerCombatManager.currentWeaponBeingUsed.fireBaseDamageAbsorption;
            player.playerStatsManager.blockingHolyAbsorption = player.playerCombatManager.currentWeaponBeingUsed.holyBaseDamageAbsorption;
            player.playerStatsManager.blockingLightningAbsorption = player.playerCombatManager.currentWeaponBeingUsed.lightningBaseDamageAbsorption;
            player.playerStatsManager.blockingStability = player.playerCombatManager.currentWeaponBeingUsed.stability;
        }
    }

    // TWO HAND WEAPON
    public void OnIsTwoHandWeaponChanged(bool oldStatus, bool newStatus)
    {
        if (!isTwoHandingWeapon.Value)
        {
            if (IsOwner)
            {
                isTwoHandingLeftWeapon.Value = false;
                isTwoHandingRightWeapon.Value = false;
            }

            player.playerEquipmentManager.UndoTwoHandWeapon();
        }

        player.anim.SetBool("IsTwoHanding", isTwoHandingWeapon.Value);
    }

    public void OnIsTwoHandingRightWeaponChanged(bool oldStatus, bool newStatus)
    {
        if (!isTwoHandingRightWeapon.Value)
            return;

        if (IsOwner)
        {
            currentWeaponBeingTwoHanded.Value = currentRightHandWeaponID.Value;
            isTwoHandingWeapon.Value = true;
        }

        player.playerInventoryManager.currentTwoHandedWeapon = player.playerInventoryManager.currentRightHandWeapon;
        player.playerEquipmentManager.TwoHandRightWeapon();
    }

    public void OnIsTwoHandingLeftWeaponChanged(bool oldStatus, bool newStatus)
    {
        if (!isTwoHandingLeftWeapon.Value)
            return;

        if (IsOwner)
        {
            currentWeaponBeingTwoHanded.Value = currentLeftHandWeaponID.Value;
            isTwoHandingWeapon.Value = true;
        }

        player.playerInventoryManager.currentTwoHandedWeapon = player.playerInventoryManager.currentLeftHandWeapon;
        player.playerEquipmentManager.TwoHandLeftWeapon();
    }
    
    // ITEM ACTION
    [ServerRpc]
    public void NotifyServerOfWeaponActionServerRpc(ulong clientID, int actionID, int weaponID)
    {
        if (IsServer)
        {
            NotifyServerOfWeaponActionClientRpc(clientID, actionID, weaponID);
        }
    }

    [ClientRpc]
    private void NotifyServerOfWeaponActionClientRpc(ulong clientID, int actionID, int weaponID)
    {
        if (clientID != NetworkManager.Singleton.LocalClientId)
        {
            PerformWeaponBasedAction(actionID, weaponID);
        }
    }

    private void PerformWeaponBasedAction(int actionID, int weaponID)
    {
        WeaponItemAction weaponAction = WorldActionManager.Instance.GetWeaponItemAction(actionID);

        if (weaponAction != null)
        {
            weaponAction.AttemptPerformAction(player, WorldItemDatabase.Instance.GetWeaponByID(weaponID));
        }
        else
        {
            Debug.LogError("ACTION is NULL");
        }
    }
}
