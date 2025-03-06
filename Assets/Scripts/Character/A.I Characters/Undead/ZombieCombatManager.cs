using UnityEngine;

public class ZombieCombatManager : AICombatManager
{
    [Header("DAMAGE COLLIDER")]
    [SerializeField] private ZombieDamageCollider rightHandDamageCollider;
    [SerializeField] private ZombieDamageCollider leftHandDamageCollider;

    [Header("BASE DAMAGE")]
    [SerializeField] private int baseDamage = 25;
    [SerializeField] private int basePoiseDamage = 25;

    [Header("ATTACK MULTIPLIER")]
    [SerializeField] private float atk01Multiplier = 1.0f;
    [SerializeField] private float atk02Multiplier = 1.2f;

    // SET DAMAGE
    public void SetAtk01Damage()
    {
        rightHandDamageCollider.physicalDamage = baseDamage * atk01Multiplier;
        leftHandDamageCollider.physicalDamage = baseDamage * atk01Multiplier;

        rightHandDamageCollider.poiseDamage = basePoiseDamage * atk01Multiplier;
        leftHandDamageCollider.poiseDamage = basePoiseDamage * atk01Multiplier;
    }

    public void SetAtk02Damage()
    {
        rightHandDamageCollider.physicalDamage = baseDamage * atk02Multiplier;
        leftHandDamageCollider.physicalDamage = baseDamage * atk02Multiplier;

        rightHandDamageCollider.poiseDamage = basePoiseDamage * atk02Multiplier;
        leftHandDamageCollider.poiseDamage = basePoiseDamage * atk02Multiplier;
    }

    // DAMAGE COLLIDER
    public void OpenRightHandDamageCollider()
    {
        aiManager.characterSoundFXManager.PlayAtkGruntSFX();
        rightHandDamageCollider.EnableDamageCollider();
    }

    public void CloseRightHandDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }

    public void OpenLeftHandDamageCollider()
    {
        aiManager.characterSoundFXManager.PlayAtkGruntSFX();
        leftHandDamageCollider.EnableDamageCollider();
    }

    public void CloseLeftHandDamageCollider()
    {
        leftHandDamageCollider.DisableDamageCollider();
    }
}
