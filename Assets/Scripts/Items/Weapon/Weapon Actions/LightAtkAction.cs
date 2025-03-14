using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack")]
public class LightAtkAction : WeaponItemAction
{
    [Header("MAIN HAND ACTION ANIM")]
    [SerializeField] private string lightAtk01 = "Main_LightAtk_01";
    [SerializeField] private string lightAtk02 = "Main_LightAtk_02";
    [SerializeField] private string runningAtk01 = "Main_Run_Attack_01";
    [SerializeField] private string rollingAtk01 = "Main_Roll_Attack_01";
    [SerializeField] private string backStepAtk01 = "Main_BackStep_Attack_01";

    [Header("TWO HAND ACTION ANIM")]
    [SerializeField] private string twoHand_lightAtk_01 = "TH_LightAtk_01";
    [SerializeField] private string twoHand_lightAtk_02 = "TH_LightAtk_02";
    [SerializeField] private string twoHand_runningAtk_01 = "TH_RunAttack_01";
    [SerializeField] private string twoHand_rollingAtk_01 = "TH_RollAttack_01";
    [SerializeField] private string twoHand_backStepAtk_01 = "TH_BackStepAttack_01";

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

        if (playerPerformingAction.characterNetworkManager.isSprinting.Value)
        {
            PerformRunningAttack(playerPerformingAction, weaponPerformingAction);
            return;
        }

        if (playerPerformingAction.characterCombatManager.canPerformRollAtk)
        {
            PerformRollingAttack(playerPerformingAction, weaponPerformingAction);
            return;
        }

        if (playerPerformingAction.characterCombatManager.canPerformBackStepAtk)
        {
            PerformBackStepAttack(playerPerformingAction, weaponPerformingAction);
            return;
        }

        PerformLightAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
        {
            PerformTwoHandLightAttack(playerPerformingAction, weaponPerformingAction);
        }
        else
        {
            PerformMainHandLightAttack(playerPerformingAction, weaponPerformingAction);
        }
    }

    private void PerformMainHandLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerCombatManager.canComboOnMainHand && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboOnMainHand = false;

            if (playerPerformingAction.playerCombatManager.lastAtkAnimPerformed == lightAtk01)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(weaponPerformingAction, AtkType.LightAtk02, lightAtk02, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(weaponPerformingAction, AtkType.LightAtk01, lightAtk01, true);
            }
        }
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(weaponPerformingAction, AtkType.LightAtk01, lightAtk01, true);
        }
    }

    private void PerformTwoHandLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerCombatManager.canComboOnMainHand && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboOnMainHand = false;

            if (playerPerformingAction.playerCombatManager.lastAtkAnimPerformed == twoHand_lightAtk_01)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(
                    weaponPerformingAction, AtkType.LightAtk02, twoHand_lightAtk_02, true);
            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(
                    weaponPerformingAction, AtkType.LightAtk01, twoHand_lightAtk_01, true);
            }
        }
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(
                weaponPerformingAction, AtkType.LightAtk01, twoHand_lightAtk_01, true);
        }
    }

    private void PerformRunningAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(
                weaponPerformingAction, AtkType.RunningAtk01, twoHand_runningAtk_01, true);
        }
        else
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(
                weaponPerformingAction, AtkType.RunningAtk01, runningAtk01, true);
        }
    }

    private void PerformRollingAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        playerPerformingAction.playerCombatManager.canPerformRollAtk = false;

        if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(
                weaponPerformingAction, AtkType.RollingAtk01, twoHand_rollingAtk_01, true);
        }
        else
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(
                weaponPerformingAction, AtkType.RollingAtk01, rollingAtk01, true);
        }
    }

    private void PerformBackStepAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        playerPerformingAction.playerCombatManager.canPerformBackStepAtk = false;

        if (playerPerformingAction.playerNetworkManager.isTwoHandingWeapon.Value)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(
                weaponPerformingAction, AtkType.BackStepAtk01, twoHand_backStepAtk_01, true);
        }
        else
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAtkActionAnim(
                weaponPerformingAction, AtkType.BackStepAtk01, backStepAtk01, true);
        }
    }
}
