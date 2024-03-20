using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EquipedItemStatus
{
    public float        increaseDamage = 1;
    public float        decreaseAttackRate = 1;
    public float        increaseAttackRange = 0;
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

        _gambleCost = ConstantData.BaseGambleCost;
    }

    public void AcquiredItem(InGameItemID inGameItemID)
    {
        InGameItemDatas.Add(Managers.Data.GetInGameItemData(inGameItemID));
        InGameItemData data = Managers.Data.GetInGameItemData(inGameItemID);
        CalculateStatusApplyEquip();
    }

    public bool CanGamble()
    {
        return Managers.Game.Ruby < _gambleCost;
    }

    public void GambleItem()
    {
        if (CanGamble())
            return;

        float randValue = UnityEngine.Random.value;
        int itemLevel = 1;
        if (randValue <= ConstantData.PercentOfLegendItem)
            itemLevel = 5;
        else if (randValue <= ConstantData.PercentOfUniqueItem)
            itemLevel = 4;
        else if (randValue <= ConstantData.PercentOfRareItem)
            itemLevel = 3;
        else if (randValue <= ConstantData.PercentOfUnCommonItem)
            itemLevel = 2;

        List<InGameItemData> list = new List<InGameItemData>();
        foreach(InGameItemData item in Managers.Data.InGameItemDict.Values)
        {
            if(item.itemLevel == itemLevel)
                list.Add(item);
        }
        int randPick = UnityEngine.Random.Range(0, list.Count);
        AcquiredItem((InGameItemID)list[randPick].id);
        Managers.Game.Ruby -= _gambleCost;
        _gambleCost += ConstantData.IncreaseGambleCost;
        Util.CheckTheEventAndCall(OnGambleItem, _gambleCost, list[randPick]);
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

        Util.CheckTheEventAndCall(OnCalculateEquipItem);
    }

    public float GetCurrentEquipedStatus(EquipItemStatus equipItemStatus)
    {
        switch (equipItemStatus)
        {
            case EquipItemStatus.increaseDamage:
                return CurrentStatusOnEquipedItem.increaseDamage;
            case EquipItemStatus.decreaseAttackRate:
                return CurrentStatusOnEquipedItem.decreaseAttackRate;
            case EquipItemStatus.increaseAttackRange:
                return CurrentStatusOnEquipedItem.increaseAttackRange;
            case EquipItemStatus.increaseAOEArea:
                return CurrentStatusOnEquipedItem.increaseAOEArea;
            case EquipItemStatus.addedDamage:
                return CurrentStatusOnEquipedItem.addedDamage;
        }
        return 0f;
    }
}
