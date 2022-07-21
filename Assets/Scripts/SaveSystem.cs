using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveStage(AudioManager audiomanager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        StageData data = new StageData(audiomanager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static StageData LoadStage(AudioManager audiomanager)
    {
        string path = Application.persistentDataPath + "/save.fun";
        if (File.Exists(path))
        {
            Debug.Log("exist");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            StageData data = formatter.Deserialize(stream) as StageData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Null");
            return new StageData(audiomanager);
            //return null;
        }
    }

    public static void NewGame()
    {
        Debug.Log("delete");
        Debug.Log(Application.persistentDataPath);

        string path = Application.persistentDataPath + "/save.fun";    
        File.Delete(path);
    }
}
