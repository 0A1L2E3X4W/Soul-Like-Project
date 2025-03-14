using TreeEditor;
using UnityEngine;

public class WeaponEquipSlot : MonoBehaviour
{
    [Header("CURRENT WEAPON")]
    public GameObject currentWeaponModel;

    [Header("HAND SLOT")]
    public WeaponSlot weaponSlot;

    public void UnloadWeapon()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }

    public void LoadWeaponToEquipSlot(GameObject weaponModel)
    {
        currentWeaponModel = weaponModel;
        weaponModel.transform.parent = transform;

        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;
        weaponModel.transform.localScale = Vector3.one;
    }

    public void LoadWeaponToUnequipSlot(GameObject weaponModel, WeaponClass weaponClass, PlayerManager player)
    {
        currentWeaponModel = weaponModel;
        weaponModel.transform.parent = transform;

        switch (weaponClass)
        {
            case WeaponClass.StraightSword:
                weaponModel.transform.localPosition = new(.064f, 0f, -.06f);
                weaponModel.transform.localRotation = Quaternion.Euler(194, 90, -.22f);
                break;
            case WeaponClass.MediumShield:
                weaponModel.transform.localPosition = new(.005f, .045f, .073f);
                weaponModel.transform.localRotation = Quaternion.Euler(-12.6f, 67f, -180f);
                break;
            default:
                break;
        }
    }
}
