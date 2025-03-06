using UnityEngine;

public class PlayerSoundFXManager : CharacterSoundFXManager
{
    [Header("MANAGER")]
    private PlayerManager player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    public override void PlayBlockingSFX()
    {
        base.PlayBlockingSFX();

        PlaySFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(player.playerCombatManager.currentWeaponBeingUsed.blockingSFX));
    }
}
