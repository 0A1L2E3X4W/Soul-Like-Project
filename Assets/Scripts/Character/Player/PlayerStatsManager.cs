using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    [Header("MANAGER")]
    private PlayerManager player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    protected override void Start()
    {
        base.Start();

        CalcuStaminaBasedOnEnduranceLV(player.playerNetworkManager.endurance.Value);
        CalcuHealthBasedOnVitalityLV(player.playerNetworkManager.vitality.Value);
    }
}
