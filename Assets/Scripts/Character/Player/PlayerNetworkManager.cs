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

        //if (player.IsOwner)
        //{
        //    PlayerUIManager.Instance.playerUIHudManager.SetRightQuickSlotIcon(newID);
        //}
    }

    public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
        player.playerInventoryManager.currentLeftHandWeapon = newWeapon;
        player.playerEquipmentManager.LoadLeftWeapon();

        //if (player.IsOwner)
        //{
        //    PlayerUIManager.Instance.playerUIHudManager.SetLeftQuickSlotIcon(newID);
        //}
    }

    public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
        player.playerCombatManager.currentWeaponBeingUsed = newWeapon;
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
}
