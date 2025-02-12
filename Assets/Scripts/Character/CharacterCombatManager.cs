using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    [Header("MANAGER")]
    protected CharacterManager character;

    [Header("ATK TARGET")]
    public CharacterManager currentTarget;

    [Header("ATK TYPE")]
    public AtkType currentAtkType;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
}
