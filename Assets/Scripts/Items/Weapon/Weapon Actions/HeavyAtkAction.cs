using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack")]
public class HeavyAtkAction : WeaponItemAction
{
    [Header("ACTION ANIM")]
    [SerializeField] private string heavyAtk01 = "Main_HeavyAtk_01";
    [SerializeField] private string heavyAtk02 = "Main_HeavyAtk_02";

    public override void AttemptPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner)
            return;

        if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            return;

        if (!playerPerformingAction.isGrounded)
            return;

        PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerCombatManager.canComboOnMainHand && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboOnMainHand = false;

            if (playerPerformingAction.playerCombatManager.lastAtkAnimPerformed == heavyAtk01)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(AtkType.HeavyAtk02, heavyAtk02, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(AtkType.HeavyAtk01, heavyAtk01, true);
            }
        }
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(AtkType.HeavyAtk01, heavyAtk01, true);
        }
    }
}
