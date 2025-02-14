using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PlayerManager : CharacterManager
{
    [Header("MANAGERS")]
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerSoundFXManager playerSoundFXManager;
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public PlayerEffectsManager playerEffectsManager;
    [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PlayerCombatManager playerCombatManager;

    [Header("DEBUG MENU")]
    [SerializeField] private bool respawnCharacter = false;

    protected override void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerSoundFXManager = GetComponent<PlayerSoundFXManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (!IsOwner)
            return;

        playerLocomotionManager.HandleAllMovement();

        playerStatsManager.RegenerateStamina();

        DebugMenu();
    }

    protected override void LateUpdate()
    {
        if (!IsOwner)
            return;

        base.LateUpdate();

        PlayerCamera.Instance.HandleAllCameraActions();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

        if (IsOwner)
        {
            PlayerCamera.Instance.player = this;
            PlayerInputManager.Instance.player = this;
            WorldSaveGameManager.Instance.player = this;

            playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthVal;
            playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaVal;

            playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.Instance.playerUIHudManager.SetNewHealthVal;
            playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.Instance.playerUIHudManager.SetNewStaminaVal;

            playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenerationTimer;
        }

        // STATS
        playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHp;

        // LOCK ON
        playerNetworkManager.isLockedOn.OnValueChanged += playerNetworkManager.OnIsLockedOnChanged;
        playerNetworkManager.currentTargetNetworkID.OnValueChanged += playerNetworkManager.OnTargetIDChanged;

        // EQUIP
        playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
        playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
        playerNetworkManager.currentWeaponBeingUsed.OnValueChanged += playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;

        // ANIM FLAGS
        playerNetworkManager.isChargingAtk.OnValueChanged += playerNetworkManager.OnIsChargingAtkChanged;

        if (IsOwner && !IsServer)
        {
            LoadGameFromCurrentCharacterData(ref WorldSaveGameManager.Instance.currentCharacterData);
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;

        if (IsOwner)
        {
            playerNetworkManager.vitality.OnValueChanged -= playerNetworkManager.SetNewMaxHealthVal;
            playerNetworkManager.endurance.OnValueChanged -= playerNetworkManager.SetNewMaxStaminaVal;

            playerNetworkManager.currentHealth.OnValueChanged -= PlayerUIManager.Instance.playerUIHudManager.SetNewHealthVal;
            playerNetworkManager.currentStamina.OnValueChanged -= PlayerUIManager.Instance.playerUIHudManager.SetNewStaminaVal;

            playerNetworkManager.currentStamina.OnValueChanged -= playerStatsManager.ResetStaminaRegenerationTimer;
        }

        // STATS
        playerNetworkManager.currentHealth.OnValueChanged -= playerNetworkManager.CheckHp;

        // LOCK ON
        playerNetworkManager.isLockedOn.OnValueChanged -= playerNetworkManager.OnIsLockedOnChanged;
        playerNetworkManager.currentTargetNetworkID.OnValueChanged -= playerNetworkManager.OnTargetIDChanged;

        // EQUIP
        playerNetworkManager.currentRightHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentRightHandWeaponIDChange;
        playerNetworkManager.currentLeftHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
        playerNetworkManager.currentWeaponBeingUsed.OnValueChanged -= playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;

        // ANIM FLAGS
        playerNetworkManager.isChargingAtk.OnValueChanged -= playerNetworkManager.OnIsChargingAtkChanged;
    }

    // SAVE & LOAD
    public void SaveGameToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;

        // CHARACTER INFO
        currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString(); // NAME
        currentCharacterData.xPos = transform.position.x; // POS X
        currentCharacterData.yPos = transform.position.y; // POS Y
        currentCharacterData.zPos = transform.position.z; // POS Z

        // RESOURCES
        currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
        currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;

        // CHARACTER STATS
        currentCharacterData.vitality = playerNetworkManager.vitality.Value;
        currentCharacterData.endurance = playerNetworkManager.endurance.Value;
    }

    public void LoadGameFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        playerNetworkManager.characterName.Value = currentCharacterData.characterName;

        Vector3 myPos = new(currentCharacterData.xPos, currentCharacterData.yPos, currentCharacterData.zPos);
        transform.position = myPos;

        playerNetworkManager.vitality.Value = currentCharacterData.vitality;
        playerNetworkManager.endurance.Value = currentCharacterData.endurance;

        playerNetworkManager.maxHealth.Value = playerStatsManager.CalcuHealthBasedOnVitalityLV(playerNetworkManager.vitality.Value);
        playerNetworkManager.maxStamina.Value = playerStatsManager.CalcuStaminaBasedOnEnduranceLV(playerNetworkManager.endurance.Value);

        playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
        playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;

        PlayerUIManager.Instance.playerUIHudManager.SetMaxStaminaVal(playerNetworkManager.maxStamina.Value);
    }

    public void LoadOtherPlayerIntoServer()
    {
        playerNetworkManager.OnCurrentRightHandWeaponIDChange(0, playerNetworkManager.currentRightHandWeaponID.Value);
        playerNetworkManager.OnCurrentLeftHandWeaponIDChange(0, playerNetworkManager.currentLeftHandWeaponID.Value);

        if (playerNetworkManager.isLockedOn.Value)
        {
            playerNetworkManager.OnTargetIDChanged(0, playerNetworkManager.currentTargetNetworkID.Value);
        }
    }

    private void OnClientConnectedCallback(ulong clientID)
    {
        WorldGameSessionManager.Instance.AddPlayerToActiveList(this);

        if (!IsServer && IsOwner)
        {
            foreach (var player in WorldGameSessionManager.Instance.players)
            {
                if (player != this)
                {
                    player.LoadOtherPlayerIntoServer();
                }
            }
        }
    }

    // DEATH & RESPAWN
    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnim = false)
    {
        if (IsOwner)
        {
            PlayerUIManager.Instance.playerUIPopUpManager.SendDeathPopUp();
        }

        return base.ProcessDeathEvent(manuallySelectDeathAnim);
    }

    public override void ReviveCharacter()
    {
        base.ReviveCharacter();

        if (IsOwner)
        {
            isDead.Value = false;
            playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
            playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;

            playerAnimatorManager.PlayTargetActionAnim("Empty", false);
        }
    }

    private void DebugMenu()
    {
        if (respawnCharacter)
        {
            respawnCharacter = false;
            ReviveCharacter();
        }
    }
}
