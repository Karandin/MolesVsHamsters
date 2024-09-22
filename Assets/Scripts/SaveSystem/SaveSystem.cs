using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/playerdata.save";

    public static void SavePlayerData(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);

        // Сериализуем данные
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadPlayerData()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);

            // Десериализуем данные
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found in " + savePath);
            return null;
        }
    }

    public static bool SaveExists()
    {
        return File.Exists(savePath);
    }
}
