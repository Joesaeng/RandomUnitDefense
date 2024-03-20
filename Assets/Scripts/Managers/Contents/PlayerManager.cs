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

        // ÀåÂøµÈ ·é ¼¼ÆÃ
        List<Rune> ownedRunes = Managers.Player.Data.ownedRunes;
        for (int i = 0; i < Managers.Player.Data.ownedRunes.Count; ++i)
        {
            if (ownedRunes[i].isEquip && ownedRunes[i].equipSlotIndex != -1)
            {
                Data.EquipedRunes[ownedRunes[i].equipSlotIndex] = ownedRunes[i];
            }
        }
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
