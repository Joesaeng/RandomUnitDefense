using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public static class Language
{
    public enum UnitInfos
    {
        Level,
        AttackDamage,
        AttackRange,
        AttackRate,
        WideAttackArea,
        SlowRatio,
        SlowDuration,
        StunDuration,
        PosionDamagePerSecond,
        PosionDuration,
    }

    public static string GetBaseUnitName(UnitNames unit)
    {
        int unitIndex = (int)unit - ConstantData.FirstOfUnitID;
        Define.GameLanguage language = Managers.Game.GameLanguage;
        string[] englishName =
        {
            "Knight",
            "Spearman",
            "Archer",
            "FireMagician",
            "SlowMagician",
            "StunGun",
            "Viking",
            "Warrior",
            "PoisonBowMan",
        };
        string[] koreanName =
        {
            "���",
            "â���",
            "�ü�",
            "���̾������",
            "���ο������",
            "���ϰ�",
            "����ŷ",
            "������",
            "�����𺸿��",
        };
        switch (language)
        {
            case Define.GameLanguage.English:
                return englishName[unitIndex];
            case Define.GameLanguage.Korean:
                return koreanName[unitIndex];
            default:
                return englishName[unitIndex];
        }
    }

    public static string GetUnitInfo(UnitInfos unitInfos)
    {
        Define.GameLanguage language = Managers.Game.GameLanguage;
        string[] englishUnitInfos =
        {
            "Level",
            "AttackDamage",
            "AttackRange",
            "AttackRate",
            "AttackArea",
            "SlowRatio",
            "Duration",
            "StunDuration",
            "PosionDamage",
            "Duration",
        };
        string[] koreaUnitInfos =
        {
            "����",
            "���ݷ�",
            "���ݹ���",
            "���ݼӵ�",
            "��������",
            "���ο�",
            "���ӽð�",
            "�������ӽð�",
            "��������",
            "���ӽð�",
        };
        switch (language)
        {
            case Define.GameLanguage.English:
                return englishUnitInfos[(int)unitInfos];
            case Define.GameLanguage.Korean:
                return koreaUnitInfos[(int)unitInfos];
            default:
                return englishUnitInfos[(int)unitInfos];
        }
    }

    public static string Sell
    {
        get {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "SELL";
                case Define.GameLanguage.Korean:
                    return "�Ǹ�";
                default:
                    return "SELL";
            }
        }
    }
}
