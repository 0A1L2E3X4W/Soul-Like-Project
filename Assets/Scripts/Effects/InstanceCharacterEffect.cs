using UnityEngine;

public class InstanceCharacterEffect : ScriptableObject
{
    [Header("EFFECT ID")]
    public int instanceEffectID;

    public virtual void ProcessEffect(CharacterManager character)
    {

    }
}
