using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    [Header("MANAGER")]
    private PlayerManager player;

    [Header("RIGHT SLOT")]
    public WeaponEquipSlot rightHandWeaponSlot;
    public GameObject rightHandWeaponModel;
    [SerializeField] private WeaponManager rightHandWeaponManager;

    [Header("LEFT SLOT")]
    public WeaponEquipSlot leftHandWeaponSlot;
    public WeaponEquipSlot leftHandShieldSlot;
    public GameObject leftHandWeaponModel;
    [SerializeField] private WeaponManager leftHandWeaponManager;

    [Header("BACK")]
    public WeaponEquipSlot backSlot;

    [Header("DEBUG")]
    [SerializeField] private bool equip = false;

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

    private void Update()
    {
        if (equip)
        {
            equip = false;
            DebugEquipItems();
        }
    }

    private void DebugEquipItems()
    {
        Debug.Log("EQUIP NEW ITEM");
        if (player.playerInventoryManager.bodyArmor != null)
            LoadBodyEquipment(player.playerInventoryManager.bodyArmor);

        if (player.playerInventoryManager.handArmor != null)
            LoadHandEquipment(player.playerInventoryManager.handArmor);

        if (player.playerInventoryManager.headArmor != null)
            LoadHeadEquipment(player.playerInventoryManager.headArmor);

        if (player.playerInventoryManager.legArmor != null)
            LoadLegEquipment(player.playerInventoryManager.legArmor);
    }

    // ARMORS
    public void LoadHeadEquipment(HeadEquipmentItem equipment)
    {
        player.playerStatsManager.CalculateTotalArmorAbsorption();
    }

    public void LoadBodyEquipment(BodyEquipmentItem equipment)
    {
        player.playerStatsManager.CalculateTotalArmorAbsorption();
    }

    public void LoadHandEquipment(HandEquipmentItem equipment)
    {
        player.playerStatsManager.CalculateTotalArmorAbsorption();
    }

    public void LoadLegEquipment(LegEquipmentItem equipment)
    {
        player.playerStatsManager.CalculateTotalArmorAbsorption();
    }

    // WEAPON
    private void InitWeaponSlots()
    {
        WeaponEquipSlot[] weaponSlots = GetComponentsInChildren<WeaponEquipSlot>();

        foreach (var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponSlot.RightHandSlot)
            {
                rightHandWeaponSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponSlot.LeftHandSlot)
            {
                leftHandWeaponSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponSlot.LeftHandShieldSlot)
            {
                leftHandShieldSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponSlot.BackSlot)
            {
                backSlot = weaponSlot;
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
            rightHandWeaponSlot.UnloadWeapon();

            // add the new weapon
            rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandWeaponSlot.LoadWeaponToEquipSlot(rightHandWeaponModel);

            rightHandWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightHandWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);

            // ANIMATOR CONTROLLER
            player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnim);
        }
    }

    public void LoadLeftWeapon()
    {
        if (player.playerInventoryManager.currentLeftHandWeapon != null)
        {
            //  REMOVE THE OLD WEAPON
            if (leftHandWeaponSlot.currentWeaponModel != null)
                leftHandWeaponSlot.UnloadWeapon();

            if (leftHandShieldSlot.currentWeaponModel != null)
                leftHandShieldSlot.UnloadWeapon();

            leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);

            switch (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType)
            {
                case WeaponModelType.Weapon:
                    leftHandWeaponSlot.LoadWeaponToEquipSlot(leftHandWeaponModel);
                    break;
                case WeaponModelType.Shield:
                    leftHandShieldSlot.LoadWeaponToEquipSlot(leftHandWeaponModel);
                    break;
                default:
                    break;
            }

            leftHandWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftHandWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);

            // ANIMATOR CONTROLLER
            player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentLeftHandWeapon.weaponAnim);
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
            rightHandWeaponManager.meleeWeaponDamageCollider.EnableDamageCollider();
            player.characterSoundFXManager.PlaySFX(
                    WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(
                        player.playerInventoryManager.currentRightHandWeapon.whooshes));
        }
        else if (player.playerNetworkManager.isUsingLeftHand.Value)
        {
            leftHandWeaponManager.meleeWeaponDamageCollider.EnableDamageCollider();
            player.characterSoundFXManager.PlaySFX(
                    WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(
                        player.playerInventoryManager.currentLeftHandWeapon.whooshes));
        }
    }

    public void CloseDamageCollider()
    {
        if (player.playerNetworkManager.isUsingRightHand.Value)
        {
            rightHandWeaponManager.meleeWeaponDamageCollider.DisableDamageCollider();
        }
        else if (player.playerNetworkManager.isUsingLeftHand.Value)
        {
            leftHandWeaponManager.meleeWeaponDamageCollider.DisableDamageCollider();
        }
    }

    // TWO HAND
    public void UndoTwoHandWeapon()
    {
        player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnim);

        if (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType == WeaponModelType.Weapon)
        {
            leftHandWeaponSlot.LoadWeaponToEquipSlot(leftHandWeaponModel);
        }
        else if (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType == WeaponModelType.Shield)
        {
            leftHandShieldSlot.LoadWeaponToEquipSlot(leftHandWeaponModel);
        }

        rightHandWeaponSlot.LoadWeaponToEquipSlot(rightHandWeaponModel);

        rightHandWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
        leftHandWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
    }

    public void TwoHandRightWeapon()
    {
        if (player.playerInventoryManager.currentRightHandWeapon == WorldItemDatabase.Instance.unarmedWeapon)
        {
            if (player.IsOwner)
            {
                player.playerNetworkManager.isTwoHandingRightWeapon.Value = false;
                player.playerNetworkManager.isTwoHandingWeapon.Value = false;
            }

            return;
        }

        // UPDATE ANIMATOR
        player.playerAnimatorManager.UpdateAnimatorController(
            player.playerInventoryManager.currentRightHandWeapon.weaponAnim);

        // ADD WEAPON TO BACK SLOT
        backSlot.LoadWeaponToUnequipSlot(
            leftHandWeaponModel, player.playerInventoryManager.currentLeftHandWeapon.weaponClass, player);

        // LOAD WEAPON TO HAND SLOT
        rightHandWeaponSlot.LoadWeaponToEquipSlot(rightHandWeaponModel);

        rightHandWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
        leftHandWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
    }
    
    public void TwoHandLeftWeapon()
    {
        if (player.playerInventoryManager.currentLeftHandWeapon == WorldItemDatabase.Instance.unarmedWeapon)
        {
            if (player.IsOwner)
            {
                player.playerNetworkManager.isTwoHandingLeftWeapon.Value = false;
                player.playerNetworkManager.isTwoHandingWeapon.Value = false;
            }

            return;
        }

        // UPDATE ANIMATOR
        player.playerAnimatorManager.UpdateAnimatorController(
            player.playerInventoryManager.currentLeftHandWeapon.weaponAnim);

        // ADD WEAPON TO BACK SLOT
        backSlot.LoadWeaponToUnequipSlot(
            rightHandWeaponModel, player.playerInventoryManager.currentRightHandWeapon.weaponClass, player);

        // LOAD WEAPON TO HAND SLOT
        rightHandWeaponSlot.LoadWeaponToEquipSlot(leftHandWeaponModel);

        rightHandWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
        leftHandWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
    }
}
