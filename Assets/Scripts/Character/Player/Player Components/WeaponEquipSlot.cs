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

    public void LoadWeapon(GameObject weaponModel)
    {
        currentWeaponModel = weaponModel;
        weaponModel.transform.parent = transform;

        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;
        weaponModel.transform.localScale = Vector3.one;
    }
}
