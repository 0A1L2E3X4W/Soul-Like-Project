using UnityEngine;

public class AIManager : CharacterManager
{
    [Header("MANAGER")]
    [HideInInspector] public AICombatManager aiCombatManager;

    [Header("CURRENT STATE")]
    [SerializeField] protected AIState currentState;

    protected override void Awake()
    {
        base.Awake();

        aiCombatManager = GetComponent<AICombatManager>();
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
    }
}
