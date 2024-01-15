using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DataManager에서 Json을 어떤 파일 포맷으로 불러들일지 저장
namespace Data
{
    #region UnitStat

    [Serializable]
    public enum UnitType
    {
        Common,     // 일반적인 공격 유닛
        AOE,        // 스플래쉬(광역) 공격 유닛
        Buffer,     // 버퍼
        Debuffer,   // 디버퍼(메저)
    }

    [Serializable]
    public enum BuffType
    {
        None,
        AttackSpeed,
        AttackDamage,
    }

    [Serializable]
    public enum DeBuffType
    {
        None,
        DefenseDecrease,
        SlowDownMoveSpeed
    }

    [Serializable]
    public class UnitStat
    {
        public int          id;             // 유닛의 동일성 판단
        public string       name;           // 이름
        public UnitType     type;           // 유닛의 직업 타입
        public int          level;          // 레벨

        public float        skillRange;     // 공격 범위 && 버프 범위
        public float        skillCoolTime;  // 공격 쿨타임 && 스킬 쿨타임

        public int          attackDamage;   // 스킬 데미지
        public float        wideAttackRange;// 광역공격 범위

        public BuffType     buffType;       // 버프유닛의 버프 타입
        public float        buffRatio;      // 버프율
        public float        buffTime;       // 버프 지속시간

        public DeBuffType   debuffType;     // 디버프유닛의 디버프 타입
        public float        debuffRatio;    // 디버프율
        public float        debuffTime;     // 디버프 지속시간
    }

    [Serializable]
    public class StatData : ILoader<int, UnitStat>
    {
        public List<UnitStat> unitStats = new List<UnitStat>();

        public Dictionary<int, UnitStat> MakeDict()
        {
            Dictionary<int,UnitStat> dict = new Dictionary<int,UnitStat>();
            foreach (UnitStat stat in unitStats)
                dict.Add(stat.id, stat);
            return dict;
        }
    }

    #endregion
}

