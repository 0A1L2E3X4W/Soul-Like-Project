using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    [Header("AUDIO SOURCE")]
    private AudioSource audioSource;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRollSoundFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.Instance.rollSFX);
    }
}
