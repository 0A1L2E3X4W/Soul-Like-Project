using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager Instance;

    [Header("MANAGER")]
    [HideInInspector] public PlayerManager player;

    [Header("WORLD SCENE INDEX")]
    [SerializeField] private int worldSceneIndex = 1;

    [Header("SAVE / LOAD")]
    [SerializeField] private bool saveGame;
    [SerializeField] private bool loadGame;

    [Header("CURRENT CHARACTER SLOT")]
    public CharacterSlot currentCharacterSlotBeingUsed;
    public CharacterSaveData currentCharacterData;

    [Header("SAVE DATA WRITER")]
    private SaveFileDataWriter saveFileDataWriter;

    [Header("CHARACTER SLOTS")]
    public CharacterSaveData characterSlot01;
    public CharacterSaveData characterSlot02;
    public CharacterSaveData characterSlot03;
    public CharacterSaveData characterSlot04;
    public CharacterSaveData characterSlot05;
    public CharacterSaveData characterSlot06;
    public CharacterSaveData characterSlot07;
    public CharacterSaveData characterSlot08;
    public CharacterSaveData characterSlot09;

    private string saveFileName;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        LoadAllCharacterProfiles();
    }

    private void Update()
    {
        if (saveGame)
        {
            saveGame = false;
            SaveGame();
        }

        if (loadGame)
        {
            loadGame = false;
            LoadGame();
        }
    }

    public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
    {
        string fileName = "";

        switch (characterSlot)
        {
            case CharacterSlot.CharacterSlot01:
                fileName = "characterSlot_01";
                break;
            case CharacterSlot.CharacterSlot02:
                fileName = "characterSlot_02";
                break;
            case CharacterSlot.CharacterSlot03:
                fileName = "characterSlot_03";
                break;
            case CharacterSlot.CharacterSlot04:
                fileName = "characterSlot_04";
                break;
            case CharacterSlot.CharacterSlot05:
                fileName = "characterSlot_05";
                break;
            case CharacterSlot.CharacterSlot06:
                fileName = "characterSlot_06";
                break;
            case CharacterSlot.CharacterSlot07:
                fileName = "characterSlot_07";
                break;
            case CharacterSlot.CharacterSlot08:
                fileName = "characterSlot_08";
                break;
            case CharacterSlot.CharacterSlot09:
                fileName = "characterSlot_09";
                break;
            default:
                break;
        }

        return fileName;
    }

    // NEW GAME
    public void AttemptCreateNewGame()
    {
        saveFileDataWriter = new();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot01);
        if (!saveFileDataWriter.CheckFileExistance())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot01;
            currentCharacterData = new();
            NewGame();

            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot02);
        if (!saveFileDataWriter.CheckFileExistance())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot02;
            currentCharacterData = new();
            NewGame();

            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot03);
        if (!saveFileDataWriter.CheckFileExistance())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot03;
            currentCharacterData = new();
            NewGame();

            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot04);
        if (!saveFileDataWriter.CheckFileExistance())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot04;
            currentCharacterData = new();
            NewGame();

            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot05);
        if (!saveFileDataWriter.CheckFileExistance())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot05;
            currentCharacterData = new();
            NewGame();

            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot06);
        if (!saveFileDataWriter.CheckFileExistance())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot06;
            currentCharacterData = new();
            NewGame();

            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot07);
        if (!saveFileDataWriter.CheckFileExistance())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot07;
            currentCharacterData = new();
            NewGame();

            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot08);
        if (!saveFileDataWriter.CheckFileExistance())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot08;
            currentCharacterData = new();
            NewGame();

            return;
        }

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot09);
        if (!saveFileDataWriter.CheckFileExistance())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot09;
            currentCharacterData = new();
            NewGame();

            return;
        }

        TitleScreenManager.Instance.DisplayNoFreeSlotPopUp();
    }

    private void NewGame()
    {
        player.playerNetworkManager.endurance.Value = 10;

        SaveGame();
        StartCoroutine(LoadWorldScene());
    }

    // LOAD GAME
    public void LoadGame()
    {
        saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

        saveFileDataWriter = new()
        {
            saveDataDirectoryPath = Application.persistentDataPath,
            saveFileName = saveFileName
        };

        currentCharacterData = saveFileDataWriter.LoadSaveGame();

        StartCoroutine(LoadWorldScene());
    }

    // SAVE GAME
    public void SaveGame()
    {
        saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

        saveFileDataWriter = new()
        {
            saveDataDirectoryPath = Application.persistentDataPath,
            saveFileName = saveFileName
        };

        player.SaveGameToCurrentCharacterData(ref currentCharacterData);

        saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
    }

    // DELETE GAME
    public void DeleteGame(CharacterSlot characterSlot)
    {
        saveFileDataWriter = new()
        {
            saveDataDirectoryPath = Application.persistentDataPath,
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot)
        };

        saveFileDataWriter.DeleteSaveFile();
    }

    private void LoadAllCharacterProfiles()
    {
        saveFileDataWriter = new();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot01);
        characterSlot01 = saveFileDataWriter.LoadSaveGame();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot02);
        characterSlot02 = saveFileDataWriter.LoadSaveGame();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot03);
        characterSlot03 = saveFileDataWriter.LoadSaveGame();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot04);
        characterSlot04 = saveFileDataWriter.LoadSaveGame();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot05);
        characterSlot05 = saveFileDataWriter.LoadSaveGame();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot06);
        characterSlot06 = saveFileDataWriter.LoadSaveGame();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot07);
        characterSlot07 = saveFileDataWriter.LoadSaveGame();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot08);
        characterSlot08 = saveFileDataWriter.LoadSaveGame();

        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot09);
        characterSlot09 = saveFileDataWriter.LoadSaveGame();
    }

    public IEnumerator LoadWorldScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
        player.LoadGameFromCurrentCharacterData(ref currentCharacterData);
        yield return null;
    }

    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }
}
