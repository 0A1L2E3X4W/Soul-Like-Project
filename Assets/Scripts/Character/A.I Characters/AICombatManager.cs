using UnityEngine;

public class AICombatManager : CharacterCombatManager
{
    [Header("MANAGER")]
    protected AIManager aiManager;

    [Header("ACTION RECOVER")]
    public float actionRecoveryTimer = 0f;

    [Header("DETECTION")]
    public float distanceFromTarget;
    [SerializeField] private float detectionRadius = 15f;
    public float minFOV = -35f;
    public float maxFOV = 35f;

    [Header("TARGET INFO")]
    public float viewableAngle;
    public Vector3 targetsDir;

    [Header("ATK ROTATION SPEED")]
    public float attackRotationSpeed = 25f;

    [Header("PIVOT")]
    public bool enablePivot = true;

    protected override void Awake()
    {
        base.Awake();

        aiManager = GetComponent<AIManager>();
        lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
    }

    public void FindTargetViaLineOfSight(AIManager aiCharacter)
    {
        if (currentTarget != null)
            return;

        Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius,
                WorldUtilityManager.Instance.GetCharacterLayers());

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            if (targetCharacter == null) { continue; }

            if (targetCharacter == aiCharacter) { continue; }

            if (targetCharacter.isDead.Value) { continue; }

            if (WorldUtilityManager.Instance.AbleAtkTarget(aiCharacter.characterGroup, targetCharacter.characterGroup))
            {
                Vector3 targetsDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                float angleOfPotentialTarget = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                if (angleOfPotentialTarget < maxFOV && angleOfPotentialTarget > minFOV)
                {
                    if (Physics.Linecast(aiCharacter.characterCombatManager.lockOnTransform.position,
                        targetCharacter.characterCombatManager.lockOnTransform.position,
                        WorldUtilityManager.Instance.GetEnvironmentLayers()))
                    {
                        Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position,
                            targetCharacter.characterCombatManager.lockOnTransform.position);
                    }
                    else
                    {
                        targetsDirection = targetCharacter.transform.position - transform.position;
                        viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, targetsDirection);
                        aiCharacter.characterCombatManager.SetTarget(targetCharacter);

                        if (enablePivot) { PivotTowardsTarget(aiCharacter); }
                    }
                }
            }
        }
    }

    public virtual void PivotTowardsTarget(AIManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return;

        if (viewableAngle >= 20f && viewableAngle <= 60)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnim("Turn_R45", true);
        }
        else if (viewableAngle <= -20f && viewableAngle >= -60)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnim("Turn_L45", true);
        }
        else if (viewableAngle >= 61 && viewableAngle <= 110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnim("Turn_R90", true);
        }
        else if (viewableAngle <= -61 && viewableAngle >= -110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnim("Turn_L90", true);
        }

        if (viewableAngle >= 110 && viewableAngle <= 145)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnim("Turn_R135", true);
        }
        else if (viewableAngle <= -110 && viewableAngle >= -145)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnim("Turn_L135", true);
        }

        if (viewableAngle >= 146 && viewableAngle <= 180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnim("Turn_R180", true);
        }
        else if (viewableAngle <= -146 && viewableAngle >= -180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnim("Turn_L180", true);
        }
    }

    public void RotateTowardsAgent(AIManager aiCharacter)
    {
        if (aiCharacter.aiNetworkManager.isMoving.Value)
        {
            aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
        }
    }

    public void RotateTowardsTargetWhileAttacking(AIManager aiCharacter)
    {
        if (currentTarget == null)
            return;

        if (!aiCharacter.canRotate)
            return;

        if (!aiCharacter.isPerformingAction)
            return;

        Vector3 targetDir = currentTarget.transform.position - aiCharacter.transform.position;
        targetDir.y = 0f;
        targetDir.Normalize();

        if (targetDir == Vector3.zero)
            targetDir = aiCharacter.transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDir);

        aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);
    }

    public void HandleActionRecover(AIManager aiCharacter)
    {
        if (actionRecoveryTimer > 0)
        {
            if (!aiCharacter.isPerformingAction)
            {
                actionRecoveryTimer -= Time.deltaTime;
            }
        }
    }
}
