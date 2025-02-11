using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    [Header("MANAGER")]
    protected CharacterManager character;

    [Header("ATK TARGET")]
    public CharacterManager currentTarget;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
}
