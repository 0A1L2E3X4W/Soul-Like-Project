using UnityEngine;

[CreateAssetMenu(menuName = "A.I/States/Idle")]
public class IdleState : AIState
{
    public override AIState Tick(AIManager aiCharacter)
    {
        if (aiCharacter.characterCombatManager.currentTarget != null)
        {
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);
        }
        else
        {
            aiCharacter.aiCombatManager.FindTargetViaLineOfSight(aiCharacter);
            return this;
        }
    }
}
