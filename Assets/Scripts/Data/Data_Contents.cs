using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DataManager���� Json�� � ���� �������� �ҷ������� ����
namespace Data
{
    #region UnitStat

    [Serializable]
    public enum UnitType
    {
        Common,     // �Ϲ����� ���� ����
        AOE,        // ���÷���(����) ���� ����
        Buffer,     // ����
        Debuffer,   // �����(����)
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

