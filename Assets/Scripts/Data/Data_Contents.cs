using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

// DataManager���� Json�� � ���� �������� �ҷ������� ����
namespace Data
{
    #region Units

    [Serializable]
    public enum UnitType
    {
        Common,     // �Ϲ����� ���� ����
        AOE,        // ����(���÷����������� �ִ�)���� ����
        Debuffer,   // �����(����)
    }

    [Serializable]
    public enum UnitNames
    {
        Knight = ConstantData.FirstOfUnitID,
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
    public enum Job
    {
        Normal,
        Bow,
        Magic,
    }

    [Serializable]
    public class BaseUnit
    {
        public int          id;                 // ���̵�
        public UnitNames    baseUnit;           // ���̽�����
        public UnitType     type;               // ���� Ÿ��
        public Job          job;                // �ִϸ��̼� ����� �̳�Ÿ��      
    }

    [Serializable]
    public class BaseUnit_Json
    {
        public int          id;                 // ���̵�
        public string       baseUnit;           // ���̽�����
        public string       type;               // ���� Ÿ��
        public string       job;                // �ִϸ��̼� ����� �̳�Ÿ��
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
                baseUnitData.baseUnit = Util.Parse<UnitNames>(data.baseUnit);
                baseUnitData.type = Util.Parse<UnitType>(data.type);
                baseUnitData.job = Util.Parse<Job>(data.job);
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

    #region Monsters

    [Serializable]
    public class MonsterData
    {
        public int          id;              // ���̵�
        public float        maxHp;           // Hp
        public int          defense;         // ����
        public float        moveSpeed;       // �̵��ӵ�
    }

    [Serializable]
    public class MonsterDatas : ILoader<int, MonsterData>
    {
        public List<MonsterData> monsters = new List<MonsterData>();

        public Dictionary<int, MonsterData> MakeDict()
        {
            Dictionary<int,MonsterData> dict = new Dictionary<int,MonsterData>();
            foreach (MonsterData data in monsters)
            {
                dict.Add(data.id, data);
            }
            return dict;
        }
    }
    #endregion
}

