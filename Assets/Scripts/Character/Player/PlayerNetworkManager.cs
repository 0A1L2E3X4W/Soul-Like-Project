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

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

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
}
