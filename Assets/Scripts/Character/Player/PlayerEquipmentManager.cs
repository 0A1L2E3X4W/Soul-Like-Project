using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    [Header("MANAGER")]
    private PlayerManager player;

    [Header("RIGHT SLOT")]
    public WeaponEquipSlot rightHandSlot;
    public GameObject rightHandWeaponModel;
    [SerializeField] private WeaponManager rightWeaponManager;

    [Header("LEFT SLOT")]
    public WeaponEquipSlot leftHandSlot;
    public GameObject leftHandWeaponModel;
    [SerializeField] private WeaponManager leftWeaponManager;

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

    // LOAD WEAPON
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

            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
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

            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }
    }

    // SWITCH WEAPON
    public void SwitchRightWeapon()
    {
        if (!player.IsOwner)
            return;

        player.playerAnimatorManager.PlayTargetActionAnim("Swap_Right_Weapon_01", false, false, true, true);

        WeaponItem selectedWeapon = null;

        player.playerInventoryManager.rightHandWeaponIndex += 1;

        if (player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
        {
            player.playerInventoryManager.rightHandWeaponIndex = 0;

            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPos = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
            {
                if (player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    weaponCount += 1;

                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                        firstWeaponPos = i;
                    }
                }
            }

            if (weaponCount <= 1)
            {
                player.playerInventoryManager.rightHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
            }
            else
            {
                player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPos;
                player.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
            }

            return;
        }

        foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots)
        {
            if (player.playerInventoryManager.weaponsInRightHandSlots
                [player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                player.playerNetworkManager.currentRightHandWeaponID.Value =
                    player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID;

                return;
            }
        }

        if (selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
        {
            SwitchRightWeapon();
        }
    }

    public void SwitchLeftWeapon()
    {
        if (!player.IsOwner)
            return;

        player.playerAnimatorManager.PlayTargetActionAnim("Swap_Left_Weapon_01", false, false, true, true);

        WeaponItem selectedWeapon = null;

        player.playerInventoryManager.leftHandWeaponIndex += 1;

        if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
        {
            player.playerInventoryManager.leftHandWeaponIndex = 0;

            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPos = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponsInLeftHandSlots.Length; i++)
            {
                if (player.playerInventoryManager.weaponsInLeftHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    weaponCount += 1;

                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[i];
                        firstWeaponPos = i;
                    }
                }
            }

            if (weaponCount <= 1)
            {
                player.playerInventoryManager.leftHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                player.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
            }
            else
            {
                player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPos;
                player.playerNetworkManager.currentLeftHandWeaponID.Value = firstWeapon.itemID;
            }

            return;
        }

        foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInLeftHandSlots)
        {
            if (player.playerInventoryManager.weaponsInLeftHandSlots
                [player.playerInventoryManager.leftHandWeaponIndex].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex];
                player.playerNetworkManager.currentLeftHandWeaponID.Value =
                    player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID;

                return;
            }
        }

        if (selectedWeapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2)
        {
            SwitchLeftWeapon();
        }
    }

    // DAMAGE COLLIDER
    public void OpenDamageCollider()
    {
        if (player.playerNetworkManager.isUsingRightHand.Value)
        {
            rightWeaponManager.meleeWeaponDamageCollider.EnableDamageCollider();
            player.characterSoundFXManager.PlaySFX(
                    WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(
                        player.playerInventoryManager.currentRightHandWeapon.whooshes));
        }
        else if (player.playerNetworkManager.isUsingLeftHand.Value)
        {
            leftWeaponManager.meleeWeaponDamageCollider.EnableDamageCollider();
            player.characterSoundFXManager.PlaySFX(
                    WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(
                        player.playerInventoryManager.currentLeftHandWeapon.whooshes));
        }
    }

    public void CloseDamageCollider()
    {
        if (player.playerNetworkManager.isUsingRightHand.Value)
        {
            rightWeaponManager.meleeWeaponDamageCollider.DisableDamageCollider();
        }
        else if (player.playerNetworkManager.isUsingLeftHand.Value)
        {
            leftWeaponManager.meleeWeaponDamageCollider.DisableDamageCollider();
        }
    }
}
