using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider
{
    [Header("CHARACTER WHO ATK")]
    public CharacterManager characterCausingDamage;

    protected override void Awake()
    {
        base.Awake();

        if (damageCollider == null)
        {
            damageCollider = GetComponent<Collider>();
        }

        damageCollider.enabled = false;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        if (damageTarget != null)
        {
            if (damageTarget == characterCausingDamage)
                return;

            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            DamageTarget(damageTarget);
        }
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
            characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

        if (characterCausingDamage.IsOwner)
        {
            //damageTarget.characterNetworkManager.NotifyServerOfCharacterDamageServerRpc(
            //    damageTarget.NetworkObjectId, characterCausingDamage.NetworkObjectId,
            //    damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.holyDamage, damageEffect.fireDamage, damageEffect.lightningDamage,
            //    damageEffect.poiseDamage, damageEffect.angleHitFrom,
            //    damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z);
        }

        //damageTarget.characterEffectsManager.ProcessInstanceEffect(damageEffect);
    }

    private void ApplyAtkDamageModifiers(float modifier, TakeDamage damage)
    {
        damage.physicalDamage *= modifier;
        damage.magicDamage *= modifier;
        damage.holyDamage *= modifier;
        damage.fireDamage *= modifier;
        damage.lightningDamage *= modifier;
        damage.poiseDamage *= modifier;
    }
}
