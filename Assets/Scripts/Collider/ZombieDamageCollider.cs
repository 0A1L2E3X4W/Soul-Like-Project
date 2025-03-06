using UnityEngine;

public class ZombieDamageCollider : DamageCollider
{
    [Header("MANAGER")]
    [SerializeField] AIManager zombieCausingDamage;

    protected override void Awake()
    {
        base.Awake();

        damageCollider = GetComponent<Collider>();
        zombieCausingDamage = GetComponentInParent<AIManager>();
    }

    protected override void DamageTarget(CharacterManager damageTarget)
    {
        base.DamageTarget(damageTarget);

        if (charactersDamaged.Contains(damageTarget))
            return;

        charactersDamaged.Add(damageTarget);

        TakeDamage damageEffect = Instantiate(WorldEffectsManager.Instance.takeDamageEffect);

        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.lightningDamage = lightningDamage;
        damageEffect.poiseDamage = poiseDamage;

        damageEffect.contactPoint = contactPoint;
        damageEffect.angleHitFrom = Vector3.SignedAngle(
            zombieCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

        if (damageTarget.IsOwner)
        {
            damageTarget.characterNetworkManager.NotifyServerOfCharacterDamageServerRpc(
                damageTarget.NetworkObjectId, zombieCausingDamage.NetworkObjectId,
                damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.holyDamage, damageEffect.fireDamage, damageEffect.lightningDamage,
                damageEffect.poiseDamage, damageEffect.angleHitFrom,
                damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z);
        }
    }

    // BLOCKING
    protected override void GetBlockingDotVal(CharacterManager damageTarget)
    {
        dirFromAtkToTarget = zombieCausingDamage.transform.position - damageTarget.transform.position;
        dotvalFromAtkToDamageTarget = Vector3.Dot(dirFromAtkToTarget, damageTarget.transform.forward);
    }
}
