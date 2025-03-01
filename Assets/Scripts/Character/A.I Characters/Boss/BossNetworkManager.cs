using UnityEngine;

public class BossNetworkManager : AINetworkManager
{
    [Header("MANAGER")]
    private BossManager bossCharacter;

    protected override void Awake()
    {
        base.Awake();

        bossCharacter = GetComponent<BossManager>();
    }

    public override void CheckHp(int oldVal, int newVal)
    {
        base.CheckHp(oldVal, newVal);

        if (bossCharacter.IsOwner)
        {
            if (currentHealth.Value <= 0)
                return;

            float hpForShifting = maxHealth.Value * (bossCharacter.hpForPhaseShift / 100);

            if (currentHealth.Value <= hpForShifting)
            {
                bossCharacter.PhaseShift();
            }
        }
    }
}
