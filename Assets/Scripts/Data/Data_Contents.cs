using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DataManager에서 Json을 어떤 파일 포맷으로 불러들일지 저장
namespace Data
{
    #region Stat

    [Serializable]
    public class Stat
    {
        public int level;
        public int maxHp;
        public int attack;
        public int totalExp;
    }

    [Serializable]
    public class StatData : ILoader<int, Stat>
    {
        public List<Stat> stats = new List<Stat>();

        public Dictionary<int, Stat> MakeDict()
        {
            Dictionary<int,Stat> dict = new Dictionary<int,Stat>();
            foreach (Stat stat in stats)
                dict.Add(stat.level, stat);
            return dict;
        }
    }

    #endregion
}

