using UnityEngine;

public class AILocomotionManager : CharacterLocomotionManager
{
    protected override void Start()
    {
        base.Start();

        yVelocity.y = groundYVelocity;
    }

    public void RotateTowardsAgent(AIManager aiCharacter)
    {
        if (aiCharacter.aiNetworkManager.isMoving.Value)
        {
            aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
        }
    }
}
