using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

// Json ������ ���� ����
namespace Data
{
    #region Player
    [Serializable]
    public class PlayerData
    {
        public bool     beginner        = true;
        public int      gameLanguage    = (int)Define.GameLanguage.English;
        public int[]    setUnits        = new int[ConstantData.SetUnitCount];
        public float    bgmVolume       = 1f;
        public float    sfxVolume       = 1f;
        public bool     bgmOn           = true;
        public bool     sfxOn           = true;
        public int      AmountOfGold    = 0;

        public  List<Rune>      ownedRunes      = new List<Rune>();
        private Rune[]          equipedRunes    = new Rune[ConstantData.EquipedRunesCount];
        public  Rune[]          EquipedRunes    { get { return equipedRunes; } set { equipedRunes = value; } }
    }

    #endregion

    #region Runes
    public enum BaseRune
    {
        Knight,
        Archer,
        Viking,
        FireMagician,
        Spearman,
        Warrior,
        PoisonBowMan,
        Rich,
        Fighter,
        Lucky,
        Curse,
        Count
    }
    public enum GradeOfRune
    {
        Common,
        Rare,
        Unique,
        Legend,
        Myth,
        None,
    }
    public enum AdditionalEffectName
    {
        IncreaseDamageOfCommon,
        IncreaseDamageOfAOE,
        InCreaseAttackSpeedOfCommon,
        InCreaseAttackSpeedOfAOE,
        CriticalChanceOfCommon,
        CriticalChanceOfAOE,
        CriticalDamageOfCommon,
        CriticalDamageOfAOE,
        AddedDamage,
        

        Count
    }

    [System.Serializable]
    public class AdditionalEffectOfRune
    {
        public AdditionalEffectName name;
        public float value;
    }

    [System.Serializable]
    public class Rune
    {
        public BaseRune baseRune;
        public GradeOfRune gradeOfRune;
        public float baseRuneEffectValue;
        public bool isEquip;
        public int equipSlotIndex = -1;
        public List<AdditionalEffectOfRune> additionalEffects = new List<AdditionalEffectOfRune>();
    }

    [System.Serializable]
    public class AdditionalEffectOfRuneValueMinMax
    {
        public string effectName;
        public float min;
        public float max;
    }

    [System.Serializable]
    public class AdditionalEffectOfRuneValueMinMaxDatas 
        : ILoader<string, AdditionalEffectOfRuneValueMinMax>
    {
        public List<AdditionalEffectOfRuneValueMinMax> effectMinMaxs 
            = new List<AdditionalEffectOfRuneValueMinMax>();

        public Dictionary<string, AdditionalEffectOfRuneValueMinMax> MakeDict()
        {
            Dictionary<string,AdditionalEffectOfRuneValueMinMax> dict 
                = new Dictionary<string,AdditionalEffectOfRuneValueMinMax>();

            foreach (AdditionalEffectOfRuneValueMinMax data in effectMinMaxs)
            {
                dict.Add(data.effectName, data);
            }
            return dict;
        }
    }

    [System.Serializable]
    public class BaseRuneValue
    {
        public string       runeGradeAndName;
        public float        value;
    }

    [System.Serializable]
    public class BaseRuneValueDatas : ILoader<string, BaseRuneValue>
    {
        public List<BaseRuneValue> RuneValues = new List<BaseRuneValue>();

        public Dictionary<string, BaseRuneValue> MakeDict()
        {
            Dictionary<string,BaseRuneValue> dict = new Dictionary<string,BaseRuneValue>();
            foreach (BaseRuneValue data in RuneValues)
            {
                dict.Add(data.runeGradeAndName, data);
            }
            return dict;
        }
    }

    #endregion

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
        Count,
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

    #region InGameItems

    public enum InGameItemID
    {
        increaseDamageItem_1 = 2000,
        increaseDamageItem_2,
        increaseDamageItem_3,
        increaseDamageItem_4,
        increaseDamageItem_5,
        attackRateItem_1,
        attackRateItem_2,
        attackRateItem_3,
        attackRateItem_4,
        attackRateItem_5,
        attackRangeItem_1,
        attackRangeItem_2,
        attackRangeItem_3,
        attackRangeItem_4,
        attackRangeItem_5,
        AOEAreaItem_1,
        AOEAreaItem_2,
        AOEAreaItem_3,
        AOEAreaItem_4,
        AOEAreaItem_5,
        addedDamageItem_1,
        addedDamageItem_2,
        addedDamageItem_3,
        addedDamageItem_4,
        addedDamageItem_5,
    }

    [Serializable]
    public class InGameItemData
    {
        public int          id;                 // ���̵�
        public int          itemLevel;          // �����۷���
        public string       itemName;           // �������̸�
        public float        increaseDamage;
        public float        decreaseAttackRate;
        public float        increaseAttackRange;
        public float        increaseAOEArea;
        public int          addedDamage;
    }

    [Serializable]
    public class InGameItemDatas : ILoader<int, InGameItemData>
    {
        public List<InGameItemData> InGameItems = new List<InGameItemData>();

        public Dictionary<int, InGameItemData> MakeDict()
        {
            Dictionary<int,InGameItemData> dict = new Dictionary<int,InGameItemData>();
            foreach (InGameItemData data in InGameItems)
            {
                dict.Add(data.id, data);
            }
            return dict;
        }
    }
    #endregion
}

