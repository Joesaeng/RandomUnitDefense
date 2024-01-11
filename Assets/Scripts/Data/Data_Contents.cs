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
    public class UnitStat
    {
        public int id;
        public UnitType type;
        public int level;
    }

    [Serializable]
    public class StatData : ILoader<int, UnitStat>
    {
        public List<UnitStat> stats = new List<UnitStat>();

        public Dictionary<int, UnitStat> MakeDict()
        {
            Dictionary<int,UnitStat> dict = new Dictionary<int,UnitStat>();
            foreach (UnitStat stat in stats)
                dict.Add(stat.level, stat);
            return dict;
        }
    }

    #endregion
}

