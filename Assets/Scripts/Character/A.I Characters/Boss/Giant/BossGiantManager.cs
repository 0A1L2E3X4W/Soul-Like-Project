using UnityEngine;

public class BossGiantManager : BossManager
{
    [Header("MANAGER")]
    [HideInInspector] public BossGiantSoundFXManager giantSoundFXManager;
    [HideInInspector] public BossGiantCombatManager giantCombatManager;

    protected override void Awake()
    {
        base.Awake();

        giantSoundFXManager = GetComponent<BossGiantSoundFXManager>();
        giantCombatManager = GetComponent<BossGiantCombatManager>();
    }
}
