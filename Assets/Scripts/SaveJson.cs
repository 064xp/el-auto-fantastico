using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class SaveJson
{
    public static void Save(string path, GameData gameData)
    {
        // string json = JsonConvert.SerializeObject(gameData);
        string json = JsonUtility.ToJson(gameData);
        System.IO.File.WriteAllText(path, json);
    }

}
