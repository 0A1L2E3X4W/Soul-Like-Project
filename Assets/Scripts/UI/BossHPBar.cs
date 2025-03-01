using TMPro;
using UnityEngine;

public class BossHPBar : StatusBar
{
    [Header("MANAGER")]
    [SerializeField] private BossManager bossCharacter;

    public void EnableBossHPBar(BossManager boss)
    {
        bossCharacter = boss;
        bossCharacter.aiNetworkManager.currentHealth.OnValueChanged += OnBossHPChange;
        SetMaxStat(bossCharacter.characterNetworkManager.maxHealth.Value);
        SetStat(bossCharacter.characterNetworkManager.currentHealth.Value);
        GetComponentInChildren<TextMeshProUGUI>().text = bossCharacter.characterName;
    }

    private void OnDestroy()
    {
        bossCharacter.aiNetworkManager.currentHealth.OnValueChanged -= OnBossHPChange;
    }

    private void OnBossHPChange(int oldVal, int newVal)
    {
        SetStat(newVal);

        if (newVal <= 0)
        {
            RemoveHpBar(2.5f);
        }
    }

    public void RemoveHpBar(float time)
    {
        Destroy(gameObject, time);
    }
}
