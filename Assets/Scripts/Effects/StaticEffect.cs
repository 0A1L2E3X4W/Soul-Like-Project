using UnityEngine;

public class StaticEffect : ScriptableObject
{
    [Header("EFFECT ID")]
    public int staticEffectID;

    public virtual void ProcessStaticEffect(CharacterManager character)
    {

    }

    public virtual void RemoveStaticEffect(CharacterManager character)
    {

    }
}
