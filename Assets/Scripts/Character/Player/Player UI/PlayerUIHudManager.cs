using UnityEngine;

public class PlayerUIHudManager : MonoBehaviour
{
    [Header("STATUS BARS")]
    [SerializeField] private StatusBar staminaBar;
    [SerializeField] private StatusBar healthBar;

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
}
