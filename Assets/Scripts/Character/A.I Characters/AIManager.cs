using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : CharacterManager
{
    [Header("MANAGER")]
    [HideInInspector] public AICombatManager aiCombatManager;
    [HideInInspector] public AINetworkManager aiNetworkManager;
    [HideInInspector] public AILocomotionManager aiLocomotionManager;

    [Header("NAV MESH AGENT")]
    [HideInInspector] public NavMeshAgent navMeshAgent;

    [Header("CURRENT STATE")]
    [SerializeField] protected AIState currentState;

    [Header("STATES")]
    public IdleState idle;
    public PursueTargetState pursueTarget;

    protected override void Awake()
    {
        base.Awake();

        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        aiCombatManager = GetComponent<AICombatManager>();
        aiNetworkManager = GetComponent<AINetworkManager>();
        aiLocomotionManager = GetComponent<AILocomotionManager>();

        idle = Instantiate(idle);
        pursueTarget = Instantiate(pursueTarget);

        currentState = idle;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (IsOwner) { ProcessStateMachine(); }
    }

    private void ProcessStateMachine()
    {
        AIState nextState = currentState?.Tick(this);

        if (nextState != null)
        {
            currentState = nextState;
        }

        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;

        if (aiCombatManager.currentTarget != null)
        {
            aiCombatManager.targetsDir = aiCombatManager.currentTarget.transform.position - transform.position;
            aiCombatManager.viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, aiCombatManager.targetsDir);
            aiCombatManager.distanceFromTarget = Vector3.Distance(transform.position, aiCombatManager.currentTarget.transform.position);
        }

        if (navMeshAgent.enabled)
        {
            Vector3 agentDestination = navMeshAgent.destination;
            float remainDistance = Vector3.Distance(agentDestination, transform.position);

            if (remainDistance > navMeshAgent.stoppingDistance)
            {
                aiNetworkManager.isMoving.Value = true;
            }
            else
            {
                aiNetworkManager.isMoving.Value = false;
            }
        }
        else
        {
            aiNetworkManager.isMoving.Value = false;
        }
    }
}
