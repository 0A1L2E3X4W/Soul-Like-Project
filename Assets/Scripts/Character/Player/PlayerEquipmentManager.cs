using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    [Header("MANAGER")]
    private PlayerManager player;

    [Header("RIGHT SLOT")]
    public WeaponEquipSlot rightHandSlot;
    public GameObject rightHandWeaponModel;

    [Header("LEFT SLOT")]
    public WeaponEquipSlot leftHandSlot;
    public GameObject leftHandWeaponModel;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();

        InitWeaponSlots();
    }

    protected override void Start()
    {
        base.Start();

        LoadWeaponOnBothHand();
    }

    private void InitWeaponSlots()
    {
        WeaponEquipSlot[] weaponSlots = GetComponentsInChildren<WeaponEquipSlot>();

        foreach (var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponSlot.RightHand)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponSlot.LeftHand)
            {
                leftHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponOnBothHand()
    {
        LoadRightWeapon();
        LoadLeftWeapon();
    }

    public void LoadRightWeapon()
    {
        if (player.playerInventoryManager.currentRightHandWeapon != null)
        {
            // remove the old weapon
            rightHandSlot.UnloadWeapon();

            // add the new weapon
            rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandSlot.LoadWeapon(rightHandWeaponModel);

            //rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            //rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
        }
    }

    public void LoadLeftWeapon()
    {
        if (player.playerInventoryManager.currentLeftHandWeapon != null)
        {
            // remove the old weapon
            leftHandSlot.UnloadWeapon();

            // add the new weapon
            leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
            leftHandSlot.LoadWeapon(leftHandWeaponModel);

            //leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            //leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }
    }
}
