using Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerManager
{
    PlayerData _playerData;
    public PlayerData Data
    {
        get 
        {
            SaveToJson();
            return _playerData;
        }
        set { _playerData = value; }
    }

    string _path;

    public void Init()
    {
        _path = Application.persistentDataPath + "/PlayerData";
        LoadFromJson();
    }

    public void SaveToJson()
    {
        File.WriteAllText(_path , JsonUtility.ToJson(_playerData));
    }

    public void LoadFromJson()
    {
        if (!File.Exists(_path))
        {
            _playerData = new PlayerData();
            SaveToJson();
        }
        string jsonData = File.ReadAllText(_path);
        _playerData = JsonUtility.FromJson<PlayerData>(jsonData);
    }
}
