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

        meleeWeaponDamageCollider.lightAtkModifier_01 = weapon.lightAtkModifier_01;
    }
}
