using System.Collections.Generic;
using UnityEngine;

public class WorldEffectsManager : MonoBehaviour
{
    public static WorldEffectsManager Instance;

    [Header("DAMAGE")]
    public TakeDamage takeDamageEffect;

    [Header("INSTANCE EFFECTS")]
    [SerializeField] List<InstanceCharacterEffect> instanceEffects;

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
    }
}
