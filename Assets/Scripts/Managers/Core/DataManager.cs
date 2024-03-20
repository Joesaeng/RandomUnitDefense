using Data;
using JetBrains.Annotations;
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
    public Dictionary<int, Data.BaseUnit> BaseUnitDict { get; private set; } = new Dictionary<int, Data.BaseUnit>();
    public Dictionary<int, Data.MonsterData> MonsterDict { get; private set; } = new Dictionary<int, Data.MonsterData>();
    public Dictionary<int, Data.InGameItemData> InGameItemDict { get; private set; } = new Dictionary<int, Data.InGameItemData>();
    public Dictionary<string, Data.BaseRuneValue> RunesDict { get; private set; } = new Dictionary<string, Data.BaseRuneValue>();
    public Dictionary<string, Data.AdditionalEffectOfRuneValueMinMax> EffectMinMaxs { get; private set; } 
        = new Dictionary<string, Data.AdditionalEffectOfRuneValueMinMax>();

    public Dictionary<int, Data.Knight> KnightStats { get; private set; } = new Dictionary<int, Data.Knight>();
    public Dictionary<int, Data.Spearman> SpearmanStats { get; private set; } = new Dictionary<int, Data.Spearman>();
    public Dictionary<int, Data.Archer> ArcherStats { get; private set; } = new Dictionary<int, Data.Archer>();
    public Dictionary<int, Data.FireMagician> FireMagicianStats { get; private set; } = new Dictionary<int, Data.FireMagician>();
    public Dictionary<int, Data.SlowMagician> SlowMagicianStats { get; private set; } = new Dictionary<int, Data.SlowMagician>();
    public Dictionary<int, Data.StunGun> StunGunStats { get; private set; } = new Dictionary<int, Data.StunGun>();
    public Dictionary<int, Data.Viking> VikingStats { get; private set; } = new Dictionary<int, Data.Viking>();
    public Dictionary<int, Data.Warrior> WarriorStats { get; private set; } = new Dictionary<int, Data.Warrior>();
    public Dictionary<int, Data.PoisonBowMan> PoisonBowManStats { get; private set; } = new Dictionary<int, Data.PoisonBowMan>();

    public void Init()
    {
        BaseUnitDict = LoadJson<Data.BaseUnitDatas, int, Data.BaseUnit>("BaseUnits").MakeDict();
        MonsterDict = LoadJson<Data.MonsterDatas, int, Data.MonsterData>("MonsterDatas").MakeDict();
        InGameItemDict = LoadJson<Data.InGameItemDatas, int, Data.InGameItemData>("InGameItemDatas").MakeDict();
        RunesDict = LoadJson<Data.BaseRuneValueDatas, string, Data.BaseRuneValue>("RuneValues").MakeDict();
        EffectMinMaxs = LoadJson<Data.AdditionalEffectOfRuneValueMinMaxDatas, string, 
            Data.AdditionalEffectOfRuneValueMinMax>("EffectMinMaxData").MakeDict();

        KnightStats = LoadJson<Data.UnitStats<Knight>, int, Data.Knight>("Knight").MakeDict();
        SpearmanStats = LoadJson<Data.UnitStats<Spearman>, int, Data.Spearman>("Spearman").MakeDict();
        ArcherStats = LoadJson<Data.UnitStats<Archer>, int, Data.Archer>("Archer").MakeDict();
        FireMagicianStats = LoadJson<Data.UnitStats<FireMagician>, int, Data.FireMagician>("FireMagician").MakeDict();
        SlowMagicianStats = LoadJson<Data.UnitStats<SlowMagician>, int, Data.SlowMagician>("SlowMagician").MakeDict();
        StunGunStats = LoadJson<Data.UnitStats<StunGun>, int, Data.StunGun>("StunGun").MakeDict();
        VikingStats = LoadJson<Data.UnitStats<Viking>, int, Data.Viking>("Viking").MakeDict();
        WarriorStats = LoadJson<Data.UnitStats<Warrior>, int, Data.Warrior>("Warrior").MakeDict();
        PoisonBowManStats = LoadJson<Data.UnitStats<PoisonBowMan>, int, Data.PoisonBowMan>("PoisonBowMan").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key,Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    public UnitStat_Base GetUnitData(UnitNames unit, int level)
    {
        switch (unit)
        {
            case UnitNames.Knight:
                return KnightStats[level];
            case UnitNames.Spearman:
                return SpearmanStats[level];
            case UnitNames.Archer:
                return ArcherStats[level];
            case UnitNames.FireMagician:
                return FireMagicianStats[level];
            case UnitNames.SlowMagician:
                return SlowMagicianStats[level];
            case UnitNames.StunGun:
                return StunGunStats[level];
            case UnitNames.Viking:
                return VikingStats[level];
            case UnitNames.Warrior:
                return WarriorStats[level];
            case UnitNames.PoisonBowMan:
                return PoisonBowManStats[level];
            default:
                return null;
        }
    }

    public MonsterData GetMonsterData(int stageNum)
    {
        return MonsterDict[stageNum + ConstantData.FirstOfMonsterID];
    }

    public InGameItemData GetInGameItemData(InGameItemID inGameItemID)
    {
        int id = (int)inGameItemID;
        return InGameItemDict[id];
    }
}
