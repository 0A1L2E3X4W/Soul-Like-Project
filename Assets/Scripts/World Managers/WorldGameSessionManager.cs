using System.Collections.Generic;
using UnityEngine;

public class WorldGameSessionManager : MonoBehaviour
{
    public static WorldGameSessionManager Instance;

    [Header("ACTIVE PLAYER IN SESSION")]
    public List<PlayerManager> players = new();

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void AddPlayerToActiveList(PlayerManager player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
        }

        for (int i = players.Count - 1; i > -1; i--)
        {
            if (players[i] == null)
            {
                players.RemoveAt(i);
            }
        }
    }

    public void RemovePlayerFromActiveList(PlayerManager player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
        }

        for (int i = players.Count - 1; i > -1; i--)
        {
            if (players[i] == null)
            {
                players.RemoveAt(i);
            }
        }
    }
}
