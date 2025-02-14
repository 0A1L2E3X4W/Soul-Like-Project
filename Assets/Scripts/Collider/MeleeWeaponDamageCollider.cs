using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider
{
    [Header("CHARACTER WHO ATK")]
    public CharacterManager characterCausingDamage;

    [Header("WEAPON ATK MODIFIERS")]
    public float lightAtkModifier_01;
    public float lightAtkModifier_02;
    public float heavyAtkModifier_01;
    public float chargedAtkModifier_01;

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

        switch (characterCausingDamage.characterCombatManager.currentAtkType)
        {
            case AtkType.LightAtk01:
                ApplyAtkDamageModifiers(lightAtkModifier_01, damageEffect);
                break;
            case AtkType.HeavyAtk01:
                ApplyAtkDamageModifiers(heavyAtkModifier_01, damageEffect);
                break;
            case AtkType.ChargedAtk01:
                ApplyAtkDamageModifiers(chargedAtkModifier_01, damageEffect);
                break;
            default:
                break;
        }

        if (characterCausingDamage.IsOwner)
        {
            damageTarget.characterNetworkManager.NotifyServerOfCharacterDamageServerRpc(
                damageTarget.NetworkObjectId, characterCausingDamage.NetworkObjectId,
                damageEffect.physicalDamage, damageEffect.magicDamage, damageEffect.holyDamage, damageEffect.fireDamage, damageEffect.lightningDamage,
                damageEffect.poiseDamage, damageEffect.angleHitFrom,
                damageEffect.contactPoint.x, damageEffect.contactPoint.y, damageEffect.contactPoint.z);
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
