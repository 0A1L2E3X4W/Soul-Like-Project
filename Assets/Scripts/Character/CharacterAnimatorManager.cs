using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    [Header("MANAGER")]
    private CharacterManager character;

    private int vertical;
    private int horizontal;

    [Header("DAMAGE ANIM")]
    [SerializeField] private string hitForwardMedium01 = "Hit_Forward_Medium_01";
    [SerializeField] private string hitForwardMedium02 = "Hit_Forward_Medium_02";
    [SerializeField] private string hitBackwardMedium01 = "Hit_Backward_Medium_01";
    [SerializeField] private string hitBackwardMedium02 = "Hit_Backward_Medium_02";
    [SerializeField] private string hitLeftMedium01 = "Hit_Left_Medium_01";
    [SerializeField] private string hitLeftMedium02 = "Hit_Left_Medium_02";
    [SerializeField] private string hitRightMedium01 = "Hit_Right_Medium_01";
    [SerializeField] private string hitRightMedium02 = "Hit_Right_Medium_02";
    [Space]
    public string finalDamageAnimPlayed;

    [Header("DAMAGE ANIM LIST")]
    public List<string> forwardMidDamage = new();
    public List<string> backwardMidDamage = new();
    public List<string> rightMidDamage = new();
    public List<string> leftMidDamage = new();

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    protected virtual void Start()
    {
        forwardMidDamage.Add(hitForwardMedium01);
        forwardMidDamage.Add(hitForwardMedium02);

        backwardMidDamage.Add(hitBackwardMedium01);
        backwardMidDamage.Add(hitBackwardMedium02);

        rightMidDamage.Add(hitRightMedium01);
        rightMidDamage.Add(hitRightMedium02);

        leftMidDamage.Add(hitLeftMedium01);
        leftMidDamage.Add(hitLeftMedium02);
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

    // PLAY ATTACK ANIM
    public virtual void PlayTargetAtkActionAnim(AtkType atkType, string targetAnim,
            bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
    {
        character.characterCombatManager.currentAtkType = atkType;
        character.characterCombatManager.lastAtkAnimPerformed = targetAnim;

        character.applyRootMotion = applyRootMotion;
        character.anim.CrossFade(targetAnim, 0.2f);
        character.isPerformingAction = isPerformingAction;
        character.canMove = canMove;
        character.canRotate = canRotate;

        character.characterNetworkManager.NotifyServerOfAtkActionAnimServerRpc(NetworkManager.Singleton.LocalClientId, targetAnim, applyRootMotion);
    }

    public string GetRandomAnimFromList(List<string> animList)
    {
        List<string> finalList = new();

        foreach (var item in animList)
        {
            finalList.Add(item);
        }

        finalList.Remove(finalDamageAnimPlayed);

        for (int i = finalList.Count - 1; i > -1; i--)
        {
            if (finalList[i] == null)
            {
                finalList.RemoveAt(i);
            }
        }

        int randomIndex = Random.Range(0, finalList.Count);

        return finalList[randomIndex];
    }
}
