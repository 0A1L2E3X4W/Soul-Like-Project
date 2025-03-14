using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack")]
public class HeavyAtkAction : WeaponItemAction
{
    [Header("MAIN HAND ACTION ANIM")]
    [SerializeField] private string heavyAtk01 = "Main_HeavyAtk_01";
    [SerializeField] private string heavyAtk02 = "Main_HeavyAtk_02";

    [Header("TWO HAND ACTION ANIM")]
    [SerializeField] private string twoHand_heavyAtk_01 = "TH_HeavyAtk_01";
    [SerializeField] private string twoHand_heavyAtk_02 = "TH_HeavyAtk_02";

    public override void AttemptPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner)
            return;

        if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            return;

        if (!playerPerformingAction.isGrounded)
            return;

        if (playerPerformingAction.IsOwner)
            playerPerformingAction.playerNetworkManager.isAttacking.Value = true;

        PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
        {
            PerformTwoHandHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }
        else
        {
            PerformMainHandHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }
    }

    private void PerformMainHandHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerCombatManager.canComboOnMainHand && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboOnMainHand = false;

            if (playerPerformingAction.playerCombatManager.lastAtkAnimPerformed == heavyAtk01)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(
                    weaponPerformingAction, AtkType.HeavyAtk02, heavyAtk02, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(
                    weaponPerformingAction, AtkType.HeavyAtk01, heavyAtk01, true);
            }
        }
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(
                weaponPerformingAction, AtkType.HeavyAtk01, heavyAtk01, true);
        }
    }

    private void PerformTwoHandHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerCombatManager.canComboOnMainHand && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboOnMainHand = false;

            if (playerPerformingAction.playerCombatManager.lastAtkAnimPerformed == twoHand_heavyAtk_01)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(
                    weaponPerformingAction, AtkType.HeavyAtk02, twoHand_heavyAtk_02, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(
                    weaponPerformingAction, AtkType.HeavyAtk01, twoHand_heavyAtk_01, true);
            }
        }
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(
                weaponPerformingAction, AtkType.HeavyAtk01, twoHand_heavyAtk_01, true);
        }
    }
}
