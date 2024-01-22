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
            "기사",
            "창기사",
            "궁수",
            "파이어매지션",
            "슬로우매지션",
            "스턴건",
            "바이킹",
            "워리어",
            "포이즌보우맨",
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
            "레벨",
            "공격력",
            "공격범위",
            "공격속도",
            "범위공격",
            "슬로우",
            "지속시간",
            "기절지속시간",
            "독데미지",
            "지속시간",
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
