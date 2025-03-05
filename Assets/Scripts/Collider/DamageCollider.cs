using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("COLLIDERS")]
    [SerializeField] protected Collider damageCollider;

    [Header("DAMAGE")]
    public float physicalDamage = 0f;
    public float magicDamage = 0f;
    public float fireDamage = 0f;
    public float lightningDamage = 0f;
    public float holyDamage = 0f;

    [Header("CONTACT POINT")]
    protected Vector3 contactPoint;

    [Header("CHARACTER DAMAGED")]
    protected List<CharacterManager> charactersDamaged = new();

    [Header("BLOCKING")]
    protected Vector3 dirFromAtkToTarget;
    protected float dotvalFromAtkToDamageTarget;

    protected virtual void Awake()
    {

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        if (damageTarget != null)
        {
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            CheckForBlock(damageTarget);
            DamageTarget(damageTarget);
        }
    }

    // DAMAGE TARGET
    protected virtual void DamageTarget(CharacterManager damageTarget)
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
        damageTarget.characterEffectsManager.ProcessInstanceEffect(damageEffect);
    }

    // DAMAGE COLLIDER
    public virtual void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public virtual void DisableDamageCollider()
    {
        damageCollider.enabled = false;
        charactersDamaged.Clear();
    }

    // BLOCKING
    protected virtual void CheckForBlock(CharacterManager damageTarget)
    {
        if (charactersDamaged.Contains(damageTarget))
            return;

        GetBlockingDotVal(damageTarget);

        if (damageTarget.characterNetworkManager.isBlocking.Value && dotvalFromAtkToDamageTarget > 0.3f)
        {
            charactersDamaged.Add(damageTarget);
            TakeBlockedDamage damageEffect = Instantiate(WorldEffectsManager.Instance.takeBlockedDamageEffect);

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;

            damageEffect.contactPoint = contactPoint;
            damageTarget.characterEffectsManager.ProcessInstanceEffect(damageEffect);
        }
    }

    protected virtual void GetBlockingDotVal(CharacterManager damageTarget)
    {
        dirFromAtkToTarget = transform.position - damageTarget.transform.position;
        dotvalFromAtkToDamageTarget = Vector3.Dot(dirFromAtkToTarget, damageTarget.transform.forward);
    }
}
