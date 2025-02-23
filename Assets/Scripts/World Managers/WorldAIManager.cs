using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager Instance;

    [Header("CHARACTERS")]
    [SerializeField] private List<AICharacterSpawner> aiCharacterSpawners;
    [SerializeField] private List<AIManager> spawnedInCharacters;

    [Header("BOSSES")]
    [SerializeField] private List<BossManager> spawnedBosses;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // SPAWN & DESPAWN
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

    // BOSSES
    public BossManager GetBossByID(int ID)
    {
        return spawnedBosses.FirstOrDefault(boss => boss.bossID == ID);
    }

    public void AddCharacterToSpawnedCharacterList(AIManager aiCharacter)
    {
        if (spawnedInCharacters.Contains(aiCharacter))
            return;

        spawnedInCharacters.Add(aiCharacter);

        BossManager boss = aiCharacter as BossManager;

        if (boss != null)
        {
            if (spawnedBosses.Contains(boss))
                return;

            spawnedBosses.Add(boss);
        }
    }
}
