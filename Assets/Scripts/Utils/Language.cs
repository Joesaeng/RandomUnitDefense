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

    public static string GetBaseUnitName(Data.BaseUnits unit, Define.GameLanguage language = Define.GameLanguage.English)
    {
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
                return englishName[(int)unit];
            case Define.GameLanguage.Korean:
                return koreanName[(int)unit];
            default:
                return englishName[(int)unit];
        }
    }

    public static string GetUnitInfo(UnitInfos unitInfos, Define.GameLanguage language = Define.GameLanguage.English)
    {
        string[] englishUnitInfos =
        {
            "Level",
            "AttackDamage",
            "AttackRange",
            "AttackRate",
            "WideAttackArea",
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
}
