using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipedItemStatus
{
    public float        increaseDamage = 1;
    public float        decreaseAttackRate = 1;
    public float        increaseAttackRange = 1;
    public float        increaseAOEArea = 1;
    public int          addedDamage = 0;
}

public class InGameItemManager
{
    public Action OnCalculateEquipItem;

    List<InGameItemData> _inGameItemDatas;
    public List<InGameItemData> InGameItemDatas
    {
        get
        { return _inGameItemDatas; }
    }

    private int _gambleCost;
    public int GambleCost => _gambleCost;
    public Action<int,InGameItemData> OnGambleItem;

    EquipedItemStatus _currentStatusOnEquipedItem;
    public EquipedItemStatus CurrentStatusOnEquipedItem
    {
        get
        { return _currentStatusOnEquipedItem; }
        set{ _currentStatusOnEquipedItem = value;}
    }
    public void Init()
    {
        if (_inGameItemDatas != null)
        {
            _inGameItemDatas = null;
        }
        _inGameItemDatas = new List<InGameItemData>();
        if (_currentStatusOnEquipedItem != null)
        {
            _currentStatusOnEquipedItem = null;
        }
        _currentStatusOnEquipedItem = new EquipedItemStatus();

        _gambleCost = ConstantData.FirstOfGambleCost;
    }

    public void AcquiredItem(InGameItemID inGameItemID)
    {
        InGameItemDatas.Add(Managers.Data.GetInGameItemData(inGameItemID));
        InGameItemData data = Managers.Data.GetInGameItemData(inGameItemID);
        CalculateStatusApplyEquip();
    }

    public void GambleItem()
    {
        if (Managers.Game.Ruby < _gambleCost)
            return;

        float randValue = UnityEngine.Random.value;
        int itemLevel = 1;
        if (randValue < ConstantData.PercentOfLegend)
            itemLevel = 5;
        else if (randValue < ConstantData.PercentOfUnique)
            itemLevel = 4;
        else if (randValue < ConstantData.PercentOfRare)
            itemLevel = 3;
        else if (randValue < ConstantData.PercentOfUnCommon)
            itemLevel = 2;

        List<InGameItemData> list = new List<InGameItemData>();
        foreach(InGameItemData item in Managers.Data.InGameItemDict.Values)
        {
            if(item.itemLevel == itemLevel)
                list.Add(item);
        }
        int randPick = UnityEngine.Random.Range(0, list.Count);
        AcquiredItem((InGameItemID)list[randPick].id);
        Debug.Log(randValue+"%");
        Managers.Game.Ruby -= _gambleCost;
        _gambleCost += ConstantData.IncreaseGambleCost;
        if (OnGambleItem != null)
            OnGambleItem.Invoke(_gambleCost, list[randPick]);
    }

    private void CalculateStatusApplyEquip()
    {
        EquipedItemStatus equipedItemStatus = new EquipedItemStatus();
        foreach(InGameItemData data in  InGameItemDatas)
        {
            equipedItemStatus.increaseDamage += data.increaseDamage;
            equipedItemStatus.decreaseAttackRate += data.decreaseAttackRate;
            equipedItemStatus.increaseAttackRange += data.increaseAttackRange;
            equipedItemStatus.increaseAOEArea += data.increaseAOEArea;
            equipedItemStatus.addedDamage += data.addedDamage;
        }

        CurrentStatusOnEquipedItem = equipedItemStatus;

        if(OnCalculateEquipItem != null)
        {
            OnCalculateEquipItem.Invoke();
        }
    }
}
