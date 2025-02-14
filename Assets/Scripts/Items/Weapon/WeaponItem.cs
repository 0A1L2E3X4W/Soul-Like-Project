using UnityEngine;

public class WeaponItem : Item
{
    [Header("WEAPON MODEL")]
    public GameObject weaponModel;

    [Header("WEAPON REQUIREMENTS")]
    public int strengthRequire = 0;
    public int dexterityRequire = 0;
    public int intelligenteRequire = 0;
    public int faithRequire = 0;

    [Header("WEAPON DAMAGE")]
    public int physicalDamage = 0;
    public int magicDamage = 0;
    public int fireDamage = 0;
    public int lightningDamage = 0;
    public int holyDamage = 0;

    [Header("POISE DAMAGE")]
    public float poiseDamage = 10f;

    [Header("ATTACK MODIFIER")]
    public float lightAtkModifier_01 = 0.8f;
    public float heavyAtkModifier_01 = 1.2f;
    public float chargedAtkModifier_01 = 1.8f;

    [Header("STAMINA COST")]
    public int baseStaminaCost = 10;
    public float lightAtkStaminaCostMultiplier = 0.8f;
    public float heavyAtkStaminaCostMultiplier = 1.2f;
    public float chargedAtkStaminaCostMultiplier = 1.8f;

    [Header("ACTIONS")]
    public WeaponItemAction oh_RB_Action;
    public WeaponItemAction oh_RT_Action;
}
