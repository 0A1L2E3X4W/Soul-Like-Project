using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "A.I/States/Combat Stance")]
public class CombatStanceState : AIState
{
    [Header("ATKS")]
    public List<AIAtkAction> aiCharacterAtks;
    private List<AIAtkAction> potentialAtks;
    private AIAtkAction choosenAtk;
    private AIAtkAction previousAtk;
    protected bool hasAtk = false;

    [Header("COMBO")]
    [SerializeField] protected bool canCombo = false;
    [SerializeField] protected float chanceOfCombo = 25f;
    protected bool hasComboChance = false;

    [Header("ENGAGEMENT DISTANCE")]
    [SerializeField] protected float maxEngageDistance = 5f;

    public override AIState Tick(AIManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return this;

        if (!aiCharacter.navMeshAgent.enabled)
            aiCharacter.navMeshAgent.enabled = true;

        if (aiCharacter.aiCombatManager.enablePivot)
        {
            if (!aiCharacter.aiNetworkManager.isMoving.Value)
            {
                if (aiCharacter.aiCombatManager.viewableAngle < -30 || aiCharacter.aiCombatManager.viewableAngle > 30)
                    aiCharacter.aiCombatManager.PivotTowardsTarget(aiCharacter);
            }
        }

        aiCharacter.aiCombatManager.RotateTowardsAgent(aiCharacter);

        if (aiCharacter.aiCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idle);

        if (!hasAtk) { GetNewAtk(aiCharacter); }
        else
        {
            aiCharacter.atk.currentAtk = choosenAtk;
            return SwitchState(aiCharacter, aiCharacter.atk);
        }

        if (aiCharacter.aiCombatManager.distanceFromTarget > maxEngageDistance)
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);

        NavMeshPath path = new();
        aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCombatManager.currentTarget.transform.position, path);
        aiCharacter.navMeshAgent.SetPath(path);

        return this;
    }

    protected virtual void GetNewAtk(AIManager aiCharacter)
    {
        potentialAtks = new List<AIAtkAction>();

        foreach (var potentialAtk in aiCharacterAtks)
        {
            if (potentialAtk.minAtkDistance > aiCharacter.aiCombatManager.distanceFromTarget)
                continue;

            if (potentialAtk.maxAtkDistance < aiCharacter.aiCombatManager.distanceFromTarget)
                continue;

            if (potentialAtk.minAtkAngle > aiCharacter.aiCombatManager.viewableAngle)
                continue;

            if (potentialAtk.maxAtkAngle < aiCharacter.aiCombatManager.viewableAngle)
                continue;

            potentialAtks.Add(potentialAtk);
        }

        if (potentialAtks.Count <= 0)
            return;

        var totalWeight = 0;

        foreach (var atk in potentialAtks)
        {
            totalWeight += atk.atkWeight;
        }

        var randomWeight = Random.Range(1, totalWeight + 1);
        var processedWeight = 0;

        foreach (var atk in potentialAtks)
        {
            processedWeight += atk.atkWeight;

            if (randomWeight <= processedWeight)
            {
                choosenAtk = atk;
                previousAtk = choosenAtk;
                hasAtk = true;
                return;
            }
        }
    }

    protected virtual bool RollForOutcomeChance(int outcomeChance)
    {
        bool willPerformOutcome = false;

        int randomPercentage = Random.Range(0, 100);

        if (randomPercentage < outcomeChance) { willPerformOutcome = true; }

        return willPerformOutcome;
    }

    protected override void ResetStateFlag(AIManager aiCharacter)
    {
        base.ResetStateFlag(aiCharacter);

        hasAtk = false;
        hasComboChance = false;
    }
}
