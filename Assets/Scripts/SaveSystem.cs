using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    public static SaveData ReadFile()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/user.save";
        SaveData saveData = new SaveData();
        FileStream stream;

        if (File.Exists(path))
        {
            try
            {
                stream = new FileStream(path, FileMode.Open);
                saveData = (SaveData)formatter.Deserialize(stream);
            }
            catch
            {
                stream = new FileStream(path, FileMode.Create);
            }
        }
        else
        {
            stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, saveData);
        }
        stream.Close();

        return saveData;
    }

    public static void WriteFile(SaveData saveData)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/user.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, saveData);
        stream.Close();
    }
}
