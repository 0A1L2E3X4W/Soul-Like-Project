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
}
