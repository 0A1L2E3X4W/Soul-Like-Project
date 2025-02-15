using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Action/Attack")]
public class AIAtkAction : ScriptableObject
{
    [Header("ATK ANIM")]
    [SerializeField] private string atkAnim;

    [Header("COMBO ACTIONS")]
    public bool actionHasCombo = false;
    public AIAtkAction comboAction;

    [Header("ACTION VALUES")]
    public int atkWeight = 50;
    [SerializeField] AtkType atkType;

    public float actionRecoveryTime = 1.5f;
    public float minAtkAngle = -35;
    public float maxAtkAngle = 35;
    public float minAtkDistance = 0;
    public float maxAtkDistance = 2;

    public void AttemptPerformAction(AIManager aiCharacter)
    {
        aiCharacter.characterAnimatorManager.PlayTargetAtkActionAnim(atkType, atkAnim, true);
    }
}
