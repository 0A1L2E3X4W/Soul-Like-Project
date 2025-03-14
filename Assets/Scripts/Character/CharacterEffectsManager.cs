using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    [Header("MANAGER")]
    private CharacterManager character;

    [Header("STATIC EFFECTS")]
    public List<StaticEffect> staticEffects = new();

    [Header("VFX")]
    [SerializeField] private GameObject bloodSplatterVFX;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstanceEffect(InstanceCharacterEffect effect)
    {
        effect.ProcessEffect(character);
    }

    public void PlayBloodSplatterVFX(Vector3 contactPoint)
    {
        if (bloodSplatterVFX != null)
        {
            GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
        }
        else
        {
            GameObject bloodSplatter = Instantiate(WorldEffectsManager.Instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
        }
    }

    // STATIC EFFECTS
    public void AddStaticEffects(StaticEffect effect)
    {
        staticEffects.Add(effect);
        effect.ProcessStaticEffect(character);

        for (int i = staticEffects.Count - 1; i > -1; i--)
        {
            if (staticEffects[i] == null) { staticEffects.RemoveAt(i); }
        }
    }

    public void RemoveStaticEffect(int effectID)
    {
        for (int i = 0; i < staticEffects.Count; i++)
        {
            if (staticEffects[i].staticEffectID == effectID)
            {
                if (staticEffects[i].staticEffectID == effectID)
                {
                    StaticEffect effect = staticEffects[i];
                    effect.RemoveStaticEffect(character);
                    staticEffects.Remove(effect);
                }
            }
        }

        for (int i = staticEffects.Count - 1; i > -1; i--)
        {
            if (staticEffects[i] == null) { staticEffects.RemoveAt(i); }
        }
    }
}
