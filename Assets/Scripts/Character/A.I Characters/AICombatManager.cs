using UnityEngine;

public class AICombatManager : CharacterCombatManager
{
    [Header("MANAGER")]
    protected AIManager aiManager;

    [Header("DETECTION")]
    public float distanceFromTarget;
    [SerializeField] private float detectionRadius = 15f;
    public float minFOV = -35f;
    public float maxFOV = 35f;

    [Header("TARGET INFO")]
    public float viewableAngle;
    public Vector3 targetsDir;

    protected override void Awake()
    {
        base.Awake();

        aiManager = GetComponent<AIManager>();
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
                        aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                    }
                }
            }
        }
    }
}
