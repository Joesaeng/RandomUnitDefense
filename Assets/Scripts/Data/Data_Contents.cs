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
        public int          id;             // ������ ���ϼ� �Ǵ�
        public string       name;           // �̸�
        public UnitType     type;           // ������ ���� Ÿ��
        public int          level;          // ����

        public float        skillRange;     // ���� ���� && ���� ����
        public float        skillCoolTime;  // ���� ��Ÿ�� && ��ų ��Ÿ��

        public int          attackDamage;   // ��ų ������
        public float        wideAttackRange;// �������� ����

        public BuffType     buffType;       // ���������� ���� Ÿ��
        public float        buffRatio;      // ������
        public float        buffTime;       // ���� ���ӽð�

        public DeBuffType   debuffType;     // ����������� ����� Ÿ��
        public float        debuffRatio;    // �������
        public float        debuffTime;     // ����� ���ӽð�
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

