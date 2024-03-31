using Data;
using System;
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
            return _playerData;
        }
        set { _playerData = value; }
    }

    public Action OnAmountOfGoldChanged;
    public int AmountOfGold
    {
        get { return _playerData.amountOfGold; }
        set
        {
            _playerData.amountOfGold = value;
            OnAmountOfGoldChanged?.Invoke();
        }
    }

    string _path;

    public void Init()
    {
        _path = Application.persistentDataPath + "/PlayerData";
        LoadFromJson();

        // ������ �� ����
        List<Rune> ownedRunes = Managers.Player.Data.ownedRunes;
        for (int i = 0; i < Managers.Player.Data.ownedRunes.Count; ++i)
        {
            if (ownedRunes[i].isEquip && ownedRunes[i].equipSlotIndex != -1)
            {
                Data.EquipedRunes[ownedRunes[i].equipSlotIndex] = ownedRunes[i];
            }
        }
    }

    // �÷��̾� �����͸� UTF-8 ���ڵ��Ͽ� ����
    public void SaveToJson()
    {
        if (File.Exists(_path))
            File.Delete(_path);

        string json = JsonUtility.ToJson(_playerData);

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);

        string encodedJson = System.Convert.ToBase64String(bytes);

        File.WriteAllText(_path, encodedJson);

    }

    // UTF-8 �����ͷ� ����� �÷��̾� �����͸� �о�� �� ���ڵ�
    public void LoadFromJson()
    {
        if (!File.Exists(_path))
        {
            _playerData = new PlayerData();

            SaveToJson();
        }

        string jsonData = File.ReadAllText(_path);

        byte[] bytes = System.Convert.FromBase64String(jsonData);

        string decodedJson = System.Text.Encoding.UTF8.GetString(bytes);

        _playerData = JsonUtility.FromJson<PlayerData>(decodedJson);
    }
}
