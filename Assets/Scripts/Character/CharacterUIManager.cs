using UnityEngine;

public class CharacterUIManager : MonoBehaviour
{
    [Header("UI")]
    public bool hasFloatingHpBar = true;
    public CharacterHPBar characterHpBar;

    public void OnHpChanged(int oldVal, int newVal)
    {
        characterHpBar.oldHpVal = oldVal;
        characterHpBar.SetStat(newVal);
    }
}
