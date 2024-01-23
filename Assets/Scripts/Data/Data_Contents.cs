using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

// DataManager에서 Json을 어떤 파일 포맷으로 불러들일지 저장
namespace Data
{
    #region BaseUnit

    [Serializable]
    public enum UnitType
    {
        Common,     // 일반적인 공격 유닛
        AOE,        // 광역(스플래쉬데미지가 있는)공격 유닛
        Debuffer,   // 디버퍼(메저)
    }

    [Serializable]
    public enum BaseUnits
    {
        Knight,
        Spearman,
        Archer,
        FireMagician,
        SlowMagician,
        StunGun,
        Viking,
        Warrior,
        PoisonBowMan,
    }

    [Serializable]
    public class BaseUnit
    {
        public int          id;                 // 아이디
        public BaseUnits    baseUnit;           // 베이스유닛
        public UnitType     type;               // 유닛 타입
    }

    [Serializable]
    public class BaseUnit_Json
    {
        public int          id;                 // 아이디
        public string       baseUnit;           // 베이스유닛
        public string       type;               // 유닛 타입
    }

    [Serializable]
    public class BaseUnitDatas : ILoader<int, BaseUnit>
    {
        public List<BaseUnit_Json> baseUnits = new List<BaseUnit_Json>();

        public Dictionary<int, BaseUnit> MakeDict()
        {
            Dictionary<int,BaseUnit> dict = new Dictionary<int,BaseUnit>();
            foreach (BaseUnit_Json data in baseUnits)
            {
                BaseUnit baseUnitData = new BaseUnit();
                baseUnitData.id = data.id;
                baseUnitData.baseUnit = Util.Parse<BaseUnits>(data.baseUnit);
                baseUnitData.type = Util.Parse<UnitType>(data.type);
                dict.Add(data.id, baseUnitData);
            }
            return dict;
        }
    }

    [Serializable]
    public class UnitStat_Base
    {
        public int level;
        public int attackDamage;
        public float attackRange;
        public float attackRate;
    }

    [Serializable]
    public class AOE : UnitStat_Base
    {
        public float wideAttackArea;
    }

    [Serializable]
    public class Knight : UnitStat_Base
    {
        
    }
    [Serializable]
    public class Spearman : UnitStat_Base
    {

    }
    [Serializable]
    public class Archer : UnitStat_Base
    {

    }
    [Serializable]
    public class FireMagician : AOE
    {
        
    }
    [Serializable]
    public class SlowMagician : AOE
    {
        public float slowRatio;
        public float slowDuration;
    }
    [Serializable]
    public class StunGun : UnitStat_Base
    {
        public float stunDuration;
    }
    [Serializable]
    public class Viking : AOE
    {

    }
    [Serializable]
    public class Warrior : AOE
    {

    }
    [Serializable]
    public class PoisonBowMan : UnitStat_Base
    {
        public float poisonDamagePerSecond;
        public float poisonDuration;
    }


    [Serializable]
    public class UnitStats<T> : ILoader<int, T> where T : UnitStat_Base
    {
        public List<T> unitStats = new List<T>();

        public Dictionary<int, T> MakeDict()
        {
            Dictionary<int,T> dict = new Dictionary<int,T>();
            foreach (T data in unitStats)
                dict.Add(data.level, data);
            return dict;
        }
    }

    #endregion
}

