using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager Instance;

    [Header("DEBUG")]
    [SerializeField] bool despawnCharacters = false;
    [SerializeField] bool spawnCharacters = false;

    [Header("CHARACTERS")]
    [SerializeField] private GameObject[] aiCharacters;
    [SerializeField] private List<GameObject> spawnedInCharacters;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (NetworkManager.Singleton.IsServer)
        {
            StartCoroutine(WaitForLoadingSceneToSpawnCharacter());
        }
    }

    private void Update()
    {
        if (spawnCharacters)
        {
            spawnCharacters = false;
            SpawnAllCharacters();
        }

        if (despawnCharacters)
        {
            despawnCharacters = false;
            DespawnAllCharacters();
        }
    }

    private IEnumerator WaitForLoadingSceneToSpawnCharacter()
    {
        while (!SceneManager.GetActiveScene().isLoaded)
        {
            yield return null;
        }

        SpawnAllCharacters();
    }

    private void SpawnAllCharacters()
    {
        foreach (var character in aiCharacters)
        {
            GameObject initantiatedCharacter = Instantiate(character);
            initantiatedCharacter.GetComponent<NetworkObject>().Spawn();
            spawnedInCharacters.Add(initantiatedCharacter);
        }
    }

    private void DespawnAllCharacters()
    {
        foreach (var character in spawnedInCharacters)
        {
            character.GetComponent<NetworkObject>().Despawn();
        }

        //spawnedInCharacters = new List<GameObject>();
    }

    private void DisableAllCharacters()
    {

    }
}
