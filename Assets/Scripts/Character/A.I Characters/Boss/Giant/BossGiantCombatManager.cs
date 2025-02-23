using UnityEngine;

public class BossGiantCombatManager : AICombatManager
{
    [Header("MANAGER")]
    private BossGiantManager giantManager;

    [Header("DAMAGE COLLIDER")]
    [SerializeField] private GiantDamageCollider giantDamageCollider;
    [SerializeField] private StompDamageCollider stompCollider;
    public float stompAttackAOERadius = 1.5f;

    [Header("DAMAGE")]
    [SerializeField] private int baseDamage = 50;
    [SerializeField] private float atk01Multiplier = 1.0f;
    [SerializeField] private float atk02Multiplier = 1.2f;
    [SerializeField] private float atk03Multiplier = 1.4f;
    public float stompDamage = 60;

    [Header("VFX")]
    public GameObject stompImpactVFX;

    protected override void Awake()
    {
        base.Awake();

        giantManager = GetComponent<BossGiantManager>();
    }

    // SET DAMAGE
    public void SetAtk01Damage()
    {
        aiManager.characterSoundFXManager.PlayAtkGruntSFX();
        giantDamageCollider.physicalDamage = baseDamage * atk01Multiplier;
    }

    public void SetAtk02Damage()
    {
        aiManager.characterSoundFXManager.PlayAtkGruntSFX();
        giantDamageCollider.physicalDamage = baseDamage * atk02Multiplier;
    }

    public void SetAtk03Damage()
    {
        aiManager.characterSoundFXManager.PlayAtkGruntSFX();
        giantDamageCollider.physicalDamage = baseDamage * atk03Multiplier;
    }

    // DAMAGE COLLIDER
    public void OpenGiantDamageCollider()
    {
        giantDamageCollider.EnableDamageCollider();

        giantManager.characterSoundFXManager.PlaySFX(
            WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(giantManager.giantSoundFXManager.whooshes));
    }

    public void CloseGiantDamageCollider()
    {
        giantDamageCollider.DisableDamageCollider();
    }

    // STOMP
    public void ActivateGiantStomp()
    {
        stompCollider.StompAttack();
    }

    public override void PivotTowardsTarget(AIManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return;

        if (viewableAngle >= 61 && viewableAngle <= 110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnim("Turn_R90", true);
        }
        else if (viewableAngle <= -61 && viewableAngle >= -110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnim("Turn_L90", true);
        }
        else if (viewableAngle >= 146 && viewableAngle <= 180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnim("Turn_R180", true);
        }
        else if (viewableAngle <= -146 && viewableAngle >= -180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnim("Turn_L180", true);
        }
    }
}
