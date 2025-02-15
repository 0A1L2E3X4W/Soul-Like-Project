using UnityEngine;

public class AIAnimatorManager : CharacterAnimatorManager
{
    private AIManager aiCharacter;

    protected override void Awake()
    {
        base.Awake();

        aiCharacter = GetComponent<AIManager>();
    }

    private void OnAnimatorMove()
    {
        if (aiCharacter.IsOwner)
        {
            if (!aiCharacter.isGrounded)
                return;

            Vector3 velocity = aiCharacter.anim.deltaPosition;

            aiCharacter.characterController.Move(velocity);
            aiCharacter.transform.rotation *= aiCharacter.anim.deltaRotation;
        }
        else
        {
            if (!aiCharacter.isGrounded)
                return;

            Vector3 velocity = aiCharacter.anim.deltaPosition;

            aiCharacter.characterController.Move(velocity);

            aiCharacter.transform.position = Vector3.SmoothDamp(
                transform.position,
                aiCharacter.aiNetworkManager.networkPosition.Value,
                ref aiCharacter.aiNetworkManager.networkPositionVelocity,
                aiCharacter.aiNetworkManager.networkPostionSmoothTime);

            aiCharacter.transform.rotation *= aiCharacter.anim.deltaRotation;
        }
    }
}
