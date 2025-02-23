using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHudManager : MonoBehaviour
{
    [Header("STATUS BARS")]
    [SerializeField] private StatusBar staminaBar;
    [SerializeField] private StatusBar healthBar;

    [Header("QUICK SLOTS")]
    [SerializeField] private Image rightQuickSlotIcon;
    [SerializeField] private Image leftQuickSlotIcon;

    [Header("BOSS HP BARS")]
    public Transform bossHpBarParent;
    public GameObject bossHpBarObj;

    public void RefreshHUD()
    {
        staminaBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(true);
    }

    // STAMINA
    public void SetNewStaminaVal(float oldVal, float newVal)
    {
        staminaBar.SetStat(Mathf.RoundToInt(newVal));
    }

    public void SetMaxStaminaVal(int maxStamina)
    {
        staminaBar.SetMaxStat(maxStamina);
    }

    // HEALTH
    public void SetNewHealthVal(int oldVal, int newVal)
    {
        healthBar.SetStat(newVal);
    }

    public void SetMaxHealthVal(int maxHealth)
    {
        healthBar.SetMaxStat(maxHealth);
    }

    // WEAPON SLOT
    public void SetRightQuickSlotIcon(int weaponID)
    {
        WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);

        if (weapon == null)
        {
            Debug.Log("ITEM NOT EXIST");

            rightQuickSlotIcon.enabled = false;
            rightQuickSlotIcon.sprite = null;
            return;
        }

        if (weapon.itemIcon == null)
        {
            Debug.Log("ITEM HAS NO ICON");

            rightQuickSlotIcon.enabled = false;
            rightQuickSlotIcon.sprite = null;
            return;
        }

        rightQuickSlotIcon.sprite = weapon.itemIcon;
        rightQuickSlotIcon.enabled = true;
    }

    public void SetLeftQuickSlotIcon(int weaponID)
    {
        WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);

        if (weapon == null)
        {
            Debug.Log("ITEM NOT EXIST");

            leftQuickSlotIcon.enabled = false;
            leftQuickSlotIcon.sprite = null;
            return;
        }

        if (weapon.itemIcon == null)
        {
            Debug.Log("ITEM HAS NO ICON");

            leftQuickSlotIcon.enabled = false;
            leftQuickSlotIcon.sprite = null;
            return;
        }

        leftQuickSlotIcon.sprite = weapon.itemIcon;
        leftQuickSlotIcon.enabled = true;
    }
}
