using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    [Header("DEBUG")]
    [SerializeField] InstanceCharacterEffect testEffect;
    [SerializeField] bool processEffect = false;

    private void Update()
    {
        if (processEffect)
        {
            processEffect = false;
            InstanceCharacterEffect effect = Instantiate(testEffect);
            ProcessInstanceEffect(effect);
        }
    }
}
