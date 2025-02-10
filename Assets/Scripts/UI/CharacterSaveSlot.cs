using TMPro;
using UnityEngine;

public class CharacterSaveSlot : MonoBehaviour
{
    [Header("SAVE FILE DATA WRITER")]
    private SaveFileDataWriter saveFileWriter;

    [Header("GAME SLOT")]
    public CharacterSlot characterSlot;

    [Header("CHARACTER INFO")]
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI timePlayed;

    private void OnEnable()
    {
        LoadSaveSlots();
    }

    private void LoadSaveSlots()
    {
        saveFileWriter = new();
        saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

        // SAVE SLOT 01
        if (characterSlot == CharacterSlot.CharacterSlot01)
        {
            saveFileWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            if (saveFileWriter.CheckFileExistance())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot01.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        // SAVE SLOT 02
        else if (characterSlot == CharacterSlot.CharacterSlot02)
        {
            saveFileWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            if (saveFileWriter.CheckFileExistance())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot02.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        // SAVE SLOT 03
        else if (characterSlot == CharacterSlot.CharacterSlot03)
        {
            saveFileWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            if (saveFileWriter.CheckFileExistance())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot03.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        // SAVE SLOT 04
        else if (characterSlot == CharacterSlot.CharacterSlot04)
        {
            saveFileWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            if (saveFileWriter.CheckFileExistance())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot04.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        // SAVE SLOT 05
        else if (characterSlot == CharacterSlot.CharacterSlot05)
        {
            saveFileWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            if (saveFileWriter.CheckFileExistance())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot05.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        // SAVE SLOT 06
        else if (characterSlot == CharacterSlot.CharacterSlot06)
        {
            saveFileWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            if (saveFileWriter.CheckFileExistance())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot06.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        // SAVE SLOT 07
        else if (characterSlot == CharacterSlot.CharacterSlot07)
        {
            saveFileWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            if (saveFileWriter.CheckFileExistance())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot07.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        // SAVE SLOT 08
        else if (characterSlot == CharacterSlot.CharacterSlot08)
        {
            saveFileWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            if (saveFileWriter.CheckFileExistance())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot08.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        // SAVE SLOT 09
        else if (characterSlot == CharacterSlot.CharacterSlot09)
        {
            saveFileWriter.saveFileName =
                WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

            if (saveFileWriter.CheckFileExistance())
            {
                characterName.text = WorldSaveGameManager.Instance.characterSlot09.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void LoadGameFromCharacterSlots()
    {
        WorldSaveGameManager.Instance.currentCharacterSlotBeingUsed = characterSlot;
        WorldSaveGameManager.Instance.LoadGame();
    }

    public void SelectCurrentSlot()
    {
        TitleScreenManager.Instance.SelectCharacterSlots(characterSlot);
    }
}
