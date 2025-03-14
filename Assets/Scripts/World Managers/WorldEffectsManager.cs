using System.Collections.Generic;
using UnityEngine;

public class WorldEffectsManager : MonoBehaviour
{
    public static WorldEffectsManager Instance;

    [Header("DAMAGE")]
    public TakeDamage takeDamageEffect;
    public TakeBlockedDamage takeBlockedDamageEffect;

    [Header("INSTANCE EFFECTS")]
    [SerializeField] List<InstanceCharacterEffect> instanceEffects;

    [Header("STATIC EFFECTS")]
    [SerializeField] List<StaticEffect> staticEffects;

    [Header("TWO HANDING")]
    public TwoHandingEffect twoHandingEffect;

    [Header("VFX")]
    public GameObject bloodSplatterVFX;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        GenerateEffectsIDs();
    }

    private void GenerateEffectsIDs()
    {
        for (int i = 0; i < instanceEffects.Count; i++)
        {
            instanceEffects[i].instanceEffectID = i;
        }

        for (int i = 0; i < staticEffects.Count; i++)
        {
            staticEffects[i].staticEffectID = i;
        }
    }
}
