using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager
{
    [Header("CURRENT SLOT")]
    public WeaponItem currentRightHandWeapon;
    public WeaponItem currentLeftHandWeapon;
    public WeaponItem currentTwoHandedWeapon;

    [Header("QUICK SLOTS")]
    public int rightHandWeaponIndex = 0;
    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[3];
    public int leftHandWeaponIndex = 0;
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[3];

    [Header("ARMORS")]
    public HeadEquipmentItem headArmor;
    public BodyEquipmentItem bodyArmor;
    public HandEquipmentItem handArmor;
    public LegsEquipmentItem legArmor;
}
