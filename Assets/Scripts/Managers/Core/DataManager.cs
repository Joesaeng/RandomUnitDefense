using Data;
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

    public Dictionary<int, Data.Knight> KnightStats { get; private set; } = new Dictionary<int, Data.Knight>();
    public Dictionary<int, Data.Spearman> SpearmanStats { get; private set; } = new Dictionary<int, Data.Spearman>();
    public Dictionary<int, Data.Archer> ArcherStats { get; private set; } = new Dictionary<int, Data.Archer>();
    public Dictionary<int, Data.FireMagician> FireMagicianStats { get; private set; } = new Dictionary<int, Data.FireMagician>();
    public Dictionary<int, Data.SlowMagician> SlowMagicianStats { get; private set; } = new Dictionary<int, Data.SlowMagician>();
    public Dictionary<int, Data.StunGun> StunGunStats { get; private set; } = new Dictionary<int, Data.StunGun>();
    public Dictionary<int, Data.Viking> VikingStats { get; private set; } = new Dictionary<int, Data.Viking>();
    public Dictionary<int, Data.Warrior> WarriorStats { get; private set; } = new Dictionary<int, Data.Warrior>();
    public Dictionary<int, Data.PoisonBowMan> PoisonBowManStats { get; private set; } = new Dictionary<int, Data.PoisonBowMan>();

    public Dictionary<BaseUnits, Dictionary<int, UnitStat_Base>> UnitStatDict { get; private set; } = new Dictionary<BaseUnits, Dictionary<int, UnitStat_Base>>();
    public void Init()
    {
        BaseUnitDict = LoadJson<Data.BaseUnitDatas, int, Data.BaseUnit>("BaseUnits").MakeDict();
        MonsterDict = LoadJson<Data.MonsterDatas, int, Data.MonsterData>("MonsterDatas").MakeDict();
       
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

    public UnitStat_Base GetUnitData(BaseUnits unit, int level)
    {
        switch (unit)
        {
            case BaseUnits.Knight:
                return KnightStats[level];
            case BaseUnits.Spearman:
                return SpearmanStats[level];
            case BaseUnits.Archer:
                return ArcherStats[level];
            case BaseUnits.FireMagician:
                return FireMagicianStats[level];
            case BaseUnits.SlowMagician:
                return SlowMagicianStats[level];
            case BaseUnits.StunGun:
                return StunGunStats[level];
            case BaseUnits.Viking:
                return VikingStats[level];
            case BaseUnits.Warrior:
                return WarriorStats[level];
            case BaseUnits.PoisonBowMan:
                return PoisonBowManStats[level];
            default:
                return null;
        }
    }

    public MonsterData GetMonsterData(int stageNum)
    {
        return MonsterDict[stageNum + ConstantData.FirstOfMonsterID];
    }
}
