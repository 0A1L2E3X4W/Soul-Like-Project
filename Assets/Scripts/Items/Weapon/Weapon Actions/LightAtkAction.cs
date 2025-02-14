using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack")]
public class LightAtkAction : WeaponItemAction
{
    [Header("ACTION ANIM")]
    [SerializeField] private string lightAtk01 = "Main_LightAtk_01";
    [SerializeField] private string lightAtk02 = "Main_LightAtk_02";

    public override void AttemptPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner)
            return;

        if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            return;

        if (!playerPerformingAction.isGrounded)
            return;

        PerformLightAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerCombatManager.canComboOnMainHand && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboOnMainHand = false;

            if (playerPerformingAction.playerCombatManager.lastAtkAnimPerformed == lightAtk01)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(AtkType.LightAtk02, lightAtk02, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(AtkType.LightAtk01, lightAtk01, true);
            }
        }
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(AtkType.LightAtk01, lightAtk01, true);
        }
    }
}
