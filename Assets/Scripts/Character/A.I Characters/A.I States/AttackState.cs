using UnityEngine;

[CreateAssetMenu(menuName = "A.I/States/Attack")]
public class AttackState : AIState
{
    [Header("CURRENT ATK")]
    [HideInInspector] public AIAtkAction currentAtk;
    [HideInInspector] public bool willPerformCombo = false;

    [Header("STATE FALGS")]
    protected bool hasPerformAtk = false;
    protected bool hasPerformCombo = false;

    [Header("PIVOT AFTER ATK")]
    [SerializeField] protected bool pivotAfterAtk = false;

    public override AIState Tick(AIManager aiCharacter)
    {
        if (aiCharacter.aiCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idle);

        if (aiCharacter.aiCombatManager.currentTarget.isDead.Value)
            return SwitchState(aiCharacter, aiCharacter.idle);

        aiCharacter.aiCombatManager.RotateTowardsTargetWhileAttacking(aiCharacter);

        aiCharacter.characterAnimatorManager.UpdateAnimatorMovementParams(0, 0, false);

        if (willPerformCombo && !hasPerformCombo)
        {
            if (currentAtk.comboAction != null)
            {
                hasPerformCombo = true;
                currentAtk.comboAction.AttemptPerformAction(aiCharacter);
            }
        }

        if (aiCharacter.isPerformingAction)
            return this;

        if (!hasPerformAtk)
        {
            if (aiCharacter.aiCombatManager.actionRecoveryTimer > 0)
                return this;

            PerformAttack(aiCharacter);

            return this;
        }

        if (pivotAfterAtk)
            aiCharacter.aiCombatManager.PivotTowardsTarget(aiCharacter);

        return SwitchState(aiCharacter, aiCharacter.combatStance);
    }

    protected void PerformAttack(AIManager aiCharacter)
    {
        hasPerformAtk = true;
        currentAtk.AttemptPerformAction(aiCharacter);
        aiCharacter.aiCombatManager.actionRecoveryTimer = currentAtk.actionRecoveryTime;
    }

    protected override void ResetStateFlag(AIManager aiCharacter)
    {
        base.ResetStateFlag(aiCharacter);

        hasPerformAtk = false;
        hasPerformCombo = false;
    }
}
