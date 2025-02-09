using Unity.Netcode;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    [Header("MANAGER")]
    private CharacterManager character;

    private int vertical;
    private int horizontal;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    // LOCOMOTION
    public void UpdateAnimatorMovementParams(float horizontalVal, float verticalVal, bool isSprinting)
    {
        float snappedHorizontal = horizontalVal;
        float snappedVertical = verticalVal;

        if (horizontalVal > 0f && horizontalVal <= 0.5f) { snappedHorizontal = 0.5f; }
        else if (horizontalVal > 0.5f && horizontalVal <= 1f) { snappedHorizontal = 1f; }
        else if (horizontalVal < -0f && horizontalVal >= -0.5f) { snappedHorizontal = -0.5f; }
        else if (horizontalVal < -0.5f && horizontalVal >= -1f) { snappedHorizontal = -1f; }
        else { snappedHorizontal = 0f; }

        if (verticalVal > 0f && verticalVal <= 0.5f) { snappedVertical = 0.5f; }
        else if (verticalVal > 0.5f && verticalVal <= 1f) { snappedVertical = 1f; }
        else if (verticalVal < -0f && verticalVal >= -0.5f) { snappedVertical = -0.5f; }
        else if (verticalVal < -0.5f && verticalVal >= -1f) { snappedVertical = -1f; }
        else { snappedVertical = 0f; }

        if (isSprinting) { snappedVertical = 2; }

        character.anim.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        character.anim.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    // PLAY ACTION ANIM
    public virtual void PlayTargetActionAnim(string targetAnim,
        bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
    {
        character.applyRootMotion = applyRootMotion;
        character.anim.CrossFade(targetAnim, 0.2f);
        character.isPerformingAction = isPerformingAction;

        character.canMove = canMove;
        character.canRotate = canRotate;

        character.characterNetworkManager.NotifyServerOfActionAnimServerRpc(NetworkManager.Singleton.LocalClientId, targetAnim, applyRootMotion);
    }
}
