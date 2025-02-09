using System.IO;
using System;
using UnityEngine;

public class SaveFileDataWriter
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    // CHECK EXISTANCE
    public bool CheckFileExistance()
    {
        if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName))) { return true; }
        else { return false; }
    }

    // DELETE FILE
    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
    }

    // CREATE FILE
    public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
    {
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("Create save file, at path: " + savePath);

            string dataToStore = JsonUtility.ToJson(characterData, true);

            using (FileStream stream = new(savePath, FileMode.Create))
            {
                using (StreamWriter fileWriter = new(stream))
                {
                    fileWriter.Write(dataToStore);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error on saving character data, details: " + ex);
        }
    }

    // LOAD FILE
    public CharacterSaveData LoadSaveGame()
    {
        CharacterSaveData characterData = null;
        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if (File.Exists(loadPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new(loadPath, FileMode.Open))
                {
                    using (StreamReader fileReader = new(stream))
                    {
                        dataToLoad = fileReader.ReadToEnd();
                    }
                }

                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogError("File is BLANK" + ex);
            }
        }

        return characterData;
    }
}
