using UnityEngine;

[CreateAssetMenu(menuName = "Character Effect/Instance Effects/Take Blocked Damage")]
public class TakeBlockedDamage : InstanceCharacterEffect
{
    [Header("CHARACTER CAUSING DAMAGE")]
    public CharacterManager characterCausingDamage;

    [Header("DAMAGE")]
    public float physicalDamage = 0f;
    public float magicDamage = 0f;
    public float fireDamage = 0f;
    public float lightningDamage = 0f;
    public float holyDamage = 0f;

    [Header("FINAL DAMAGE")]
    private int finalDamageDealt = 0;

    [Header("POISE")]
    public float poiseDamage = 0f;
    public bool poiseIsBroken = false;

    [Header("ANIMATIONS")]
    public bool playDamageAnim = true;
    public bool manuallySelectDamageAnim = false;
    public string damageAnim;

    public bool playDamageSFX = true;
    public AudioClip elementalDamageSFX;

    [Header("DIRECTION OF DAMAGE TAKEN FROM")]
    public float angleHitFrom;
    public Vector3 contactPoint;

    public override void ProcessEffect(CharacterManager character)
    {
        if (character.characterNetworkManager.isInvulenerable.Value)
            return;

        base.ProcessEffect(character);

        Debug.Log("DAMAGE BLOCKED");

        if (character.isDead.Value)
            return;

        CalcuDamage(character);
        PlayDirectionalBasedBlockedDamageAnim(character);
        PlayDamageSFX(character);
        PlayDamageVFX(character);
    }

    private void CalcuDamage(CharacterManager character)
    {
        if (!character.IsOwner)
            return;

        if (characterCausingDamage != null)
        {

        }

        Debug.Log("Origin Damage: " + physicalDamage);

        physicalDamage -= (physicalDamage * (character.characterStatsManager.blockingPhysicalAbsorption / 100));
        magicDamage -= (magicDamage * (character.characterStatsManager.blockingMagicAbsorption / 100));
        fireDamage -= (fireDamage * (character.characterStatsManager.blockingFireAbsorption / 100));
        lightningDamage -= (lightningDamage * (character.characterStatsManager.blockingLightningAbsorption / 100));
        holyDamage -= (holyDamage * (character.characterStatsManager.blockingHolyAbsorption / 100));

        finalDamageDealt =
            Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

        if (finalDamageDealt <= 0)
            finalDamageDealt = 1;

        Debug.Log("Final Damage: " + physicalDamage);

        character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
    }

    private void PlayDamageVFX(CharacterManager character)
    {
    }

    private void PlayDamageSFX(CharacterManager character)
    {
    }

    private void PlayDirectionalBasedBlockedDamageAnim(CharacterManager character)
    {
        if (!character.IsOwner)
            return;

        if (character.isDead.Value)
            return;

        DamageIntensity damageIntensity = WorldUtilityManager.Instance.GetDamageIntensityBasedOnPoiseDamage(poiseDamage);

        switch (damageIntensity)
        {
            case DamageIntensity.Ping:
                damageAnim = "Block_Ping_01";
                break;
            case DamageIntensity.Light:
                damageAnim = "Block_Light_01";
                break;
            case DamageIntensity.Medium:
                damageAnim = "Block_Medium_01";
                break;
            case DamageIntensity.Heavy:
                damageAnim = "Block_Heavy_01";
                break;
            case DamageIntensity.Colossal:
                damageAnim = "Block_Colossal_01";
                break;
            default:
                break;
        }

        character.characterAnimatorManager.finalDamageAnimPlayed = damageAnim;
        character.characterAnimatorManager.PlayTargetActionAnim(damageAnim, true);
    }
}
