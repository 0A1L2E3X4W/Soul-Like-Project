using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WorldObjectManager : MonoBehaviour
{
    public static WorldObjectManager Instance;

    [Header("FOG WALLS")]
    public List<FogWallsInteractable> fogWalls;

    [Header("OJECTS")]
    [SerializeField] private List<NetworkObjectSpawner> networkObjSpawners;
    [SerializeField] private List<GameObject> spawnedInObjs;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    // OBJECTS
    public void SpawnObject(NetworkObjectSpawner networkObjSpawner)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            networkObjSpawners.Add(networkObjSpawner);
            networkObjSpawner.AttemptSpawnCharacters();
        }
    }

    // FOG WALLS
    public void AddFogWallToList(FogWallsInteractable fogWall)
    {
        if (!fogWalls.Contains(fogWall))
        {
            fogWalls.Add(fogWall);
        }
    }

    public void RemoveFogWallFromList(FogWallsInteractable fogWall)
    {
        if (fogWalls.Contains(fogWall))
        {
            fogWalls.Remove(fogWall);
        }
    }
}
