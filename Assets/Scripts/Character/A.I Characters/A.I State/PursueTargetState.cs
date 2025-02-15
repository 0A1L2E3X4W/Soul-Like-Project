using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "A.I/States/Pursue Target")]
public class PursueTargetState : AIState
{
    public override AIState Tick(AIManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction) { return this; }

        if (aiCharacter.aiCombatManager.currentTarget == null) { return SwitchState(aiCharacter, aiCharacter.idle); }

        if (!aiCharacter.navMeshAgent.enabled) { aiCharacter.navMeshAgent.enabled = true; }

        if (aiCharacter.aiCombatManager.enablePivot)
        {
            if (aiCharacter.aiCombatManager.viewableAngle < aiCharacter.aiCombatManager.minFOV ||
            aiCharacter.aiCombatManager.viewableAngle > aiCharacter.aiCombatManager.maxFOV)
                aiCharacter.aiCombatManager.PivotTowardsTarget(aiCharacter);
        }

        aiCharacter.aiLocomotionManager.RotateTowardsAgent(aiCharacter);

        //if (aiCharacter.aiCombatManager.distanceFromTarget <= aiCharacter.navMeshAgent.stoppingDistance)
        //    return SwitchState(aiCharacter, aiCharacter.combatStance);

        NavMeshPath path = new();
        aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCombatManager.currentTarget.transform.position, path);
        aiCharacter.navMeshAgent.SetPath(path);

        return this;
    }
}
