using UnityEngine;

public class WeaponItem : Item
{
    [Header("WEAPON MODEL")]
    public GameObject weaponModel;

    [Header("MODEL INSTANTIATION")]
    public WeaponModelType weaponModelType;

    [Header("WEAPON CLASS")]
    public WeaponClass weaponClass;

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
    public float lightAtkModifier_02 = 1.0f;
    public float heavyAtkModifier_01 = 1.2f;
    public float heavyAtkModifier_02 = 1.4f;
    public float chargedAtkModifier_01 = 1.8f;
    public float chargedAtkModifier_02 = 2.0f;

    public float runningAtkModifier_01 = 1.5f;
    public float rollingAtkModifier_01 = 1.4f;
    public float backstepAtkModifier_01 = 1.4f;

    [Header("STAMINA COST")]
    public int baseStaminaCost = 10;
    public float lightAtkStaminaCostMultiplier = 0.8f;
    public float heavyAtkStaminaCostMultiplier = 1.2f;
    public float chargedAtkStaminaCostMultiplier = 1.8f;
    public float runningAtkStaminaCostMultiplier = 1.3f;
    public float rollingAtkStaminaCostMultiplier = 1.3f;
    public float backstepAtkStaminaCostMultiplier = 1.3f;

    [Header("WEAPON BLOCKING ABSORPTIONS")]
    public float physicalBaseDamageAbsorption = 50;
    public float magicBaseDamageAbsorption = 50;
    public float fireBaseDamageAbsorption = 50;
    public float holyBaseDamageAbsorption = 50;
    public float lightningBaseDamageAbsorption = 50;
    public float stability = 50;    // REDUCES STAMINA LOST FROM BLOCK 

    [Header("ACTIONS")]
    public WeaponItemAction oh_RB_Action;
    public WeaponItemAction oh_RT_Action;
    public WeaponItemAction oh_LB_Action;

    [Header("ANIMATIONS")]
    public AnimatorOverrideController weaponAnim;

    [Header("SOUND FX")]
    public AudioClip[] whooshes;
    public AudioClip[] blockingSFX;
}
