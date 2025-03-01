using System.Collections;
using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager Instance;

    [Header("ACTION SOUNDS")]
    public AudioClip rollSFX;

    [Header("DAMAGE SOUND")]
    public AudioClip[] physicalSFX;

    [Header("BOSS TRACK")]
    [SerializeField] AudioSource bossIntroPlayer;
    [SerializeField] AudioSource bossLoopPlayer;

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

    // BOSS FIGHT SFX
    public void PlayBossTrack(AudioClip introTrack, AudioClip loopTrack)
    {
        bossIntroPlayer.volume = 1;
        bossIntroPlayer.clip = introTrack;
        bossIntroPlayer.loop = false;
        bossIntroPlayer.Play();

        bossLoopPlayer.volume = 1;
        bossLoopPlayer.clip = loopTrack;
        bossLoopPlayer.loop = true;
        bossLoopPlayer.PlayDelayed(bossIntroPlayer.clip.length);
    }

    public void StopBossMusic()
    {
        StartCoroutine(FadeOutBossMusic());
    }

    private IEnumerator FadeOutBossMusic()
    {
        while (bossLoopPlayer.volume > 0)
        {
            bossLoopPlayer.volume -= Time.deltaTime;
            bossIntroPlayer.volume -= Time.deltaTime;
            yield return null;
        }

        bossIntroPlayer.Stop();
        bossLoopPlayer.Stop();
    }
}
