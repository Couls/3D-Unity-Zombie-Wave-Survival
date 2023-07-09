using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MenuHandler : MonoBehaviour
{
    string savePath;
    void Awake()
    {
        savePath = Application.persistentDataPath + "/saveData/";
        Debug.Log("Save path: " + savePath);
    }
    public static void SaveWaveManagerAsJSON(string path, string filename, WaveManager WM)
    {
        // if the folder does not exist, create it
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // convert the object to JSON
        string json = JsonUtility.ToJson(WM);

        // save the JSOn to the save file
        File.WriteAllText(path + filename, json);
    }
    void HelloWorld() { Debug.Log("Hello"); }
    public static WaveManager LoadWaveManagerFromJSON(string path, string filename)
    {
        if (File.Exists(path + filename))
        {
            // read the JSON from the file
            string json = File.ReadAllText(path + filename);

            // convert the JSON into an object
            WaveManager WM = GameObject.Find("GameManager").GetComponent<WaveManager>();
            JsonUtility.FromJsonOverwrite(json, WM);
            return WM;
        }
        else
        {
            Debug.LogError("Cannot find file: " + path + filename);
            return null;
        }
    }
    public void SaveGame()
    {
        Debug.Log("Saving WaveData...");
        SaveWaveManagerAsJSON(savePath, "WaveData.json", GameObject.Find("GameManager").GetComponent<WaveManager>());
    }
    public void LoadGame()
    {
        Debug.Log("Loading WaveData...");
        WaveManager toLoad = GameObject.Find("GameManager").GetComponent<WaveManager>();
        toLoad = LoadWaveManagerFromJSON(savePath, "WaveData.json");
        toLoad.UpdateData();
    }
}
