using UnityEngine;

[CreateAssetMenu(menuName = "Character Effect/Instance Effects/Take Damage")]
public class TakeDamage : InstanceCharacterEffect
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

    [Header("DIRECTION OF DAMAGE TAKEN FROM")]
    public float angleHitFrom;
    public Vector3 contactPoint;

    [Header("ANIMATIONS")]
    public bool playDamageAnim = true;
    public bool manuallySelectDamageAnim = false;
    public string damageAnim;

    [Header("SOUND FX")]
    public bool playDamageSFX = true;
    public AudioClip elementalDamageSFX;

    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);

        if (character.isDead.Value)
            return;

        CalcuDamage(character);
        PlayDirectionalBasedDamageAnim(character);
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

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

        if (finalDamageDealt <= 0) { finalDamageDealt = 1; }

        character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
        Debug.Log("TAKE DAMAGE: " + finalDamageDealt);
    }

    private void PlayDamageVFX(CharacterManager character)
    {
        character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
    }

    private void PlayDamageSFX(CharacterManager character)
    {
        AudioClip physicalSFX = WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(WorldSoundFXManager.Instance.physicalSFX);

        character.characterSoundFXManager.PlaySFX(physicalSFX);
    }

    private void PlayDirectionalBasedDamageAnim(CharacterManager character)
    {
        if (!character.IsOwner)
            return;

        if (character.isDead.Value)
            return;

        poiseIsBroken = true;

        if (angleHitFrom >= 145 && angleHitFrom <= 180) // FRONT
        {
            damageAnim = character.characterAnimatorManager.GetRandomAnimFromList(character.characterAnimatorManager.forwardMidDamage);
        }
        else if (angleHitFrom <= -145 && angleHitFrom >= -180) // FRONT
        {
            damageAnim = character.characterAnimatorManager.GetRandomAnimFromList(character.characterAnimatorManager.forwardMidDamage);
        }
        else if (angleHitFrom >= -45 && angleHitFrom <= 45) // BACK
        {
            damageAnim = character.characterAnimatorManager.GetRandomAnimFromList(character.characterAnimatorManager.backwardMidDamage);
        }
        else if (angleHitFrom >= -144 && angleHitFrom <= -45) // LEFT
        {
            damageAnim = character.characterAnimatorManager.GetRandomAnimFromList(character.characterAnimatorManager.leftMidDamage);
        }
        else if (angleHitFrom >= 45 && angleHitFrom <= 144) // RIGHT
        {
            damageAnim = character.characterAnimatorManager.GetRandomAnimFromList(character.characterAnimatorManager.rightMidDamage);
        }

        if (poiseIsBroken)
        {
            character.characterAnimatorManager.finalDamageAnimPlayed = damageAnim;
            character.characterAnimatorManager.PlayTargetActionAnim(damageAnim, true);
        }
    }
}
