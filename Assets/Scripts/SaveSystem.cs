using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    public static UserData ReadFile()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.bin";
        UserData userData;

        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);

            userData = (UserData)formatter.Deserialize(stream);
        }
        else
        {
            FileStream stream = new FileStream(path, FileMode.Create);

            userData = new UserData();

            formatter.Serialize(stream, userData);
        }

        return userData;
    }

    public static void WriteFile(UserData userData)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.bin";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, userData);
        stream.Close();
    }
}
