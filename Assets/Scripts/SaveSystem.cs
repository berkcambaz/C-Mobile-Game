using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    public static Player ReadFile()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.bin";
        Player player;

        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);

            player = (Player)formatter.Deserialize(stream);
        }
        else
        {
            FileStream stream = new FileStream(path, FileMode.Create);

            player = new Player();

            formatter.Serialize(stream, player);
        }

        return player;
    }

    public static void WriteFile(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.bin";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, player);
        stream.Close();
    }
}
