using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager 
{
    public Dictionary<int, Data.UnitStat> StatDict { get; private set; } = new Dictionary<int, Data.UnitStat>();

    public void Init()
    {
        StatDict = LoadJson<Data.StatData, int, Data.UnitStat>("unitStatData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key,Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
