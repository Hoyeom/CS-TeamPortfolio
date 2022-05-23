using System;
using System.IO;
using UnityEngine;

public class DataManager
{
    [Serializable]
    class Data
    {
        public int coin;
        public int bestScore;
    }

    private string DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";
    Data data = new Data();
    public void Initialize()
    {
        DATA_DIRECTORY = Application.streamingAssetsPath + "/Save/";
        
        if (!Directory.Exists(DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_FILENAME);
        LoadData();
    }
    
    public void SaveData()
    {
        data.coin = Managers.Game.Coin;
        data.bestScore = Managers.Game.BestScore;

        string json = JsonUtility.ToJson(data);
        Debug.Log(DATA_DIRECTORY + SAVE_FILENAME);
        File.WriteAllText(DATA_DIRECTORY + SAVE_FILENAME,json);
    }

    public void LoadData()
    {
        if (File.Exists(DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(DATA_DIRECTORY + SAVE_FILENAME);
            data = JsonUtility.FromJson<Data>(loadJson);
            Managers.Game.Coin = data.coin;
            Managers.Game.BestScore = data.bestScore;
        }
    }
    
}