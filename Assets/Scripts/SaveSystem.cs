using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveStage(AudioManager audiomanager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        StageData data = new StageData(audiomanager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static StageData LoadStage(AudioManager audiomanager)
    {
        string path = Application.persistentDataPath + "/player.fun";
        if (!File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            StageData data = formatter.Deserialize(stream) as StageData;
            stream.Close();

            return data;
        }
        else
        {
            return new StageData(audiomanager);
        }
    }
}
