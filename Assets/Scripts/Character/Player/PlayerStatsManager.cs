using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    [Header("MANAGER")]
    private PlayerManager player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    protected override void Start()
    {
        base.Start();

        CalcuStaminaBasedOnEnduranceLV(player.playerNetworkManager.endurance.Value);
        CalcuHealthBasedOnVitalityLV(player.playerNetworkManager.vitality.Value);
    }

    public void CalculateTotalArmorAbsorption()
    {
        armorPhysicalDamageAbsorption = 0;
        armorMagicalDamageAbsorption = 0;
        armorFireDamageAbsorption = 0;
        armorLightningDamageAbsorption = 0;
        armorHolyDamageAbsorption = 0;

        armorImmunity = 0;
        armorRobustness = 0;
        armorFocus = 0;
        armorVitality = 0;

        basePoiseDefense = 0;

        if (player.playerInventoryManager.headArmor != null)
        {
            armorPhysicalDamageAbsorption += player.playerInventoryManager.headArmor.physicalDamageAbsorption;
            armorMagicalDamageAbsorption += player.playerInventoryManager.headArmor.magicalDamageAbsorption;
            armorFireDamageAbsorption += player.playerInventoryManager.headArmor.fireDamageAbsorption;
            armorLightningDamageAbsorption += player.playerInventoryManager.headArmor.lightningDamageAbsorption;
            armorHolyDamageAbsorption += player.playerInventoryManager.headArmor.holyDamageAbsorption;

            armorImmunity += player.playerInventoryManager.headArmor.immunity;
            armorFocus += player.playerInventoryManager.headArmor.focus;
            armorRobustness += player.playerInventoryManager.headArmor.robustness;
            armorVitality += player.playerInventoryManager.headArmor.vitality;

            basePoiseDefense += player.playerInventoryManager.headArmor.poise;
        }

        if (player.playerInventoryManager.bodyArmor != null)
        {
            armorPhysicalDamageAbsorption += player.playerInventoryManager.bodyArmor.physicalDamageAbsorption;
            armorMagicalDamageAbsorption += player.playerInventoryManager.bodyArmor.magicalDamageAbsorption;
            armorFireDamageAbsorption += player.playerInventoryManager.bodyArmor.fireDamageAbsorption;
            armorLightningDamageAbsorption += player.playerInventoryManager.bodyArmor.lightningDamageAbsorption;
            armorHolyDamageAbsorption += player.playerInventoryManager.bodyArmor.holyDamageAbsorption;

            armorImmunity += player.playerInventoryManager.bodyArmor.immunity;
            armorFocus += player.playerInventoryManager.bodyArmor.focus;
            armorRobustness += player.playerInventoryManager.bodyArmor.robustness;
            armorVitality += player.playerInventoryManager.bodyArmor.vitality;

            basePoiseDefense += player.playerInventoryManager.bodyArmor.poise;
        }

        if (player.playerInventoryManager.handArmor != null)
        {
            armorPhysicalDamageAbsorption += player.playerInventoryManager.handArmor.physicalDamageAbsorption;
            armorMagicalDamageAbsorption += player.playerInventoryManager.handArmor.magicalDamageAbsorption;
            armorFireDamageAbsorption += player.playerInventoryManager.handArmor.fireDamageAbsorption;
            armorLightningDamageAbsorption += player.playerInventoryManager.handArmor.lightningDamageAbsorption;
            armorHolyDamageAbsorption += player.playerInventoryManager.handArmor.holyDamageAbsorption;

            armorImmunity += player.playerInventoryManager.handArmor.immunity;
            armorFocus += player.playerInventoryManager.handArmor.focus;
            armorRobustness += player.playerInventoryManager.handArmor.robustness;
            armorVitality += player.playerInventoryManager.handArmor.vitality;

            basePoiseDefense += player.playerInventoryManager.handArmor.poise;
        }

        if (player.playerInventoryManager.legArmor != null)
        {
            armorPhysicalDamageAbsorption += player.playerInventoryManager.legArmor.physicalDamageAbsorption;
            armorMagicalDamageAbsorption += player.playerInventoryManager.legArmor.magicalDamageAbsorption;
            armorFireDamageAbsorption += player.playerInventoryManager.legArmor.fireDamageAbsorption;
            armorLightningDamageAbsorption += player.playerInventoryManager.legArmor.lightningDamageAbsorption;
            armorHolyDamageAbsorption += player.playerInventoryManager.legArmor.holyDamageAbsorption;

            armorImmunity += player.playerInventoryManager.legArmor.immunity;
            armorFocus += player.playerInventoryManager.legArmor.focus;
            armorRobustness += player.playerInventoryManager.legArmor.robustness;
            armorVitality += player.playerInventoryManager.legArmor.vitality;

            basePoiseDefense += player.playerInventoryManager.legArmor.poise;
        }
    }
}
