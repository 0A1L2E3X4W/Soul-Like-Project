using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    [Header("MANAGER")]
    private CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstanceEffect(InstanceCharacterEffect effect)
    {
        effect.ProcessEffect(character);
    }
}
