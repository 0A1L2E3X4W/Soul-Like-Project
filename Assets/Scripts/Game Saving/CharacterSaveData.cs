using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    [Header("WORLD INDEX")]
    public int sceneIndex = 1;

    [Header("CHARACTER NAME")]
    public string characterName = "Character";

    [Header("TIME PLAYED")]
    public float secondsPlayed;

    [Header("WORLD COORDINATES")]
    public float xPos;
    public float yPos;
    public float zPos;

    [Header("STATS")]
    public int vitality;
    public int endurance;

    [Header("RESOURCES")]
    public int currentHealth;
    public float currentStamina;

    [Header("BOSS")]
    public SerializableDictionary<int, bool> bossesAwakened;
    public SerializableDictionary<int, bool> bossesDefeated;

    [Header("CHECK POINTS")]
    public SerializableDictionary<int, bool> checkpoint;

    public CharacterSaveData()
    {
        checkpoint = new SerializableDictionary<int, bool>();
        bossesAwakened = new SerializableDictionary<int, bool>();
        bossesDefeated = new SerializableDictionary<int, bool>();
    }
}
