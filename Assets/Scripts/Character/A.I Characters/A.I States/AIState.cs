using UnityEngine;

public class AIState : ScriptableObject
{
    public virtual AIState Tick(AIManager aiCharacter)
    {
        return this;
    }

    protected virtual AIState SwitchState(AIManager aiCharacter, AIState newState)
    {
        ResetStateFlag(aiCharacter);
        return newState;
    }

    protected virtual void ResetStateFlag(AIManager aiCharacter)
    {

    }
}
