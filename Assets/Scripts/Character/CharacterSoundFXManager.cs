using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    [Header("AUDIO SOURCE")]
    private AudioSource audioSource;

    [Header("DAMAGE GRUNTS")]
    [SerializeField] protected AudioClip[] damageGrunts;

    [Header("ATK GRUNTS")]
    [SerializeField] protected AudioClip[] atkGrunts;

    [Header("FOOT STEP")]
    [SerializeField] protected AudioClip[] footSteps;

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

    public virtual void PlayDamageGruntSFX()
    {
        if (damageGrunts.Length > 0)
            PlaySFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(damageGrunts));
    }

    public virtual void PlayAtkGruntSFX()
    {
        if (atkGrunts.Length > 0)
            PlaySFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(atkGrunts));
    }

    public virtual void PlayFootStepSFX()
    {
        if (footSteps.Length > 0)
            PlaySFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(footSteps));
    }
}
