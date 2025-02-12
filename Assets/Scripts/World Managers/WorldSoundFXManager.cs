using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager Instance;

    [Header("ACTION SOUNDS")]
    public AudioClip rollSFX;

    [Header("DAMAGE SOUND")]
    public AudioClip[] physicalSFX;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public AudioClip ChooseRandomSFXFromArray(AudioClip[] audios)
    {
        int index = Random.Range(0, audios.Length);
        return audios[index];
    }
}
