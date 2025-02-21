using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager Instance;

    [Header("CHARACTERS")]
    [SerializeField] private List<AICharacterSpawner> aiCharacterSpawners;
    [SerializeField] private List<GameObject> spawnedInCharacters;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SpawnCharacter(AICharacterSpawner aiSpawner)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            aiCharacterSpawners.Add(aiSpawner);
            aiSpawner.AttemptSpawnCharacters();
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
