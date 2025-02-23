using UnityEngine;

public class BossGiantSoundFXManager : CharacterSoundFXManager
{
    [Header("WHOOSHES")]
    public AudioClip[] whooshes;

    [Header("WEAPON IMPACTS")]
    public AudioClip[] weaponImpacts;

    [Header("STOMP IMPACTS")]
    public AudioClip[] stompImpacts;

    public virtual void PlayWeaponImpactSFX()
    {
        if (weaponImpacts.Length > 0)
            PlaySFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(weaponImpacts));
    }

    public virtual void PlayStompImpactSFX()
    {
        if (stompImpacts.Length > 0)
            PlaySFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(stompImpacts));
    }
}
