using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("DAMAGE COLLIDER")]
    public MeleeWeaponDamageCollider meleeWeaponDamageCollider;

    private void Awake()
    {
        meleeWeaponDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
    {
        meleeWeaponDamageCollider.characterCausingDamage = characterWieldingWeapon;

        meleeWeaponDamageCollider.physicalDamage = weapon.physicalDamage;
        meleeWeaponDamageCollider.magicDamage = weapon.magicDamage;
        meleeWeaponDamageCollider.fireDamage = weapon.fireDamage;
        meleeWeaponDamageCollider.lightningDamage = weapon.lightningDamage;
        meleeWeaponDamageCollider.holyDamage = weapon.holyDamage;
        meleeWeaponDamageCollider.poiseDamage = weapon.poiseDamage;

        meleeWeaponDamageCollider.lightAtkModifier_01 = weapon.lightAtkModifier_01;
        meleeWeaponDamageCollider.lightAtkModifier_02 = weapon.lightAtkModifier_02;
        meleeWeaponDamageCollider.heavyAtkModifier_01 = weapon.heavyAtkModifier_01;
        meleeWeaponDamageCollider.heavyAtkModifier_02 = weapon.heavyAtkModifier_02;
        meleeWeaponDamageCollider.chargedAtkModifier_01 = weapon.chargedAtkModifier_01;
        meleeWeaponDamageCollider.chargedAtkModifier_02 = weapon.chargedAtkModifier_02;

        meleeWeaponDamageCollider.runningAtkModifier_01 = weapon.runningAtkModifier_01;
        meleeWeaponDamageCollider.rollingAtkModifier_01 = weapon.rollingAtkModifier_01;
        meleeWeaponDamageCollider.backstepAtkModifier_01 = weapon.backstepAtkModifier_01;
    }
}
