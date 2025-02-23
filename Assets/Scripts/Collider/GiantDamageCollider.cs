using UnityEngine;

public class GiantDamageCollider : DamageCollider
{
    [Header("MANAGER")]
    [SerializeField] BossManager boss;

    protected override void Awake()
    {
        base.Awake();

        damageCollider = GetComponent<Collider>();
        boss = GetComponentInParent<BossManager>();
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

        damageEffect.contactPoint = contactPoint;
        damageEffect.angleHitFrom = Vector3.SignedAngle(
            boss.transform.forward, damageTarget.transform.forward, Vector3.up);

        if (damageTarget.IsOwner)
        {
            damageTarget.characterNetworkManager.NotifyServerOfCharacterDamageServerRpc(
                damageTarget.NetworkObjectId, boss.NetworkObjectId,
                damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.holyDamage, damageEffect.fireDamage, damageEffect.lightningDamage,
                damageEffect.poiseDamage, damageEffect.angleHitFrom,
                damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z);
        }
    }
}
