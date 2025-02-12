using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    [Header("AUDIO SOURCE")]
    private AudioSource audioSource;

    [Header("DAMAGE GRUNTS")]
    [SerializeField] protected AudioClip[] damageGrunts;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
    {
        audioSource.PlayOneShot(soundFX, volume);
        audioSource.pitch = 1;

        if (randomizePitch)
        {
            audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
    }

    public void PlayRollSoundFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.Instance.rollSFX);
    }
}
