using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class JsonHelper{

    public static GameData[] FromJson<GameData>(string json){
            Wrapper<GameData> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<GameData>>(json);
        return wrapper.Items;
    }

    public static string ToJson<GameData>(List<GameData> array){
        Wrapper<GameData> wrapper = new Wrapper<GameData>();
        wrapper.Items = array.ToArray();
        return UnityEngine.JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<GameData>{
        public GameData[] Items;
    }
}