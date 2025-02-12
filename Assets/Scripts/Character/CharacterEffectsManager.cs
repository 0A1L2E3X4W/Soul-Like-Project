using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    [Header("MANAGER")]
    private CharacterManager character;

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
}
