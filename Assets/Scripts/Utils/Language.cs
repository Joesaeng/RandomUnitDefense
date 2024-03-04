using Data;
using static Define;

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

    public static string GetItemInfo(ItemName itemname)
    {
        Define.GameLanguage language = Managers.Game.GameLanguage;
        string[] englishItemInfos =
        {
            "",
            "Increase Damage",
            "Decrease Attack Rate",
            "Increase Attack Range",
            "Increase AOE Area",
            "Added Damage"

        };
        string[] koreanItemInfos =
        {
            "",
            "공격력 증가",
            "공격 주기 감소",
            "공격범위 증가",
            "광역공격범위 증가",
            "추가 데미지"
        };
        switch (language)
        {
            case Define.GameLanguage.English:
                return englishItemInfos[(int)itemname];
            case Define.GameLanguage.Korean:
                return koreanItemInfos[(int)itemname];
            default:
                return englishItemInfos[(int)itemname];
        }
    }

    public static string GetEquipStatusName(EquipItemStatus equipItemStatus)
    {
        Define.GameLanguage language = Managers.Game.GameLanguage;
        string[] englishItemInfos =
        {
            "Increase Damage",
            "Decrease Attack Rate",
            "Increase Attack Range",
            "Increase AOE Area",
            "Added Damage"

        };
        string[] koreanItemInfos =
        {
            "공격력 증가",
            "공격 주기 감소",
            "공격범위 증가",
            "광역공격범위 증가",
            "추가 데미지"
        };
        switch (language)
        {
            case Define.GameLanguage.English:
                return englishItemInfos[(int)equipItemStatus];
            case Define.GameLanguage.Korean:
                return koreanItemInfos[(int)equipItemStatus];
            default:
                return englishItemInfos[(int)equipItemStatus];
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
                    return "판매";
                default:
                    return "SELL";
            }
        }
    }

    public static string Spawn
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "SPAWN";
                case Define.GameLanguage.Korean:
                    return "유닛소환";
                default:
                    return "SPAWN";
            }
        }
    }

    public static string GambleItem
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Gamble\n" +
                            "Item";
                case Define.GameLanguage.Korean:
                    return "아이템\n" +
                            "뽑기";
                default:
                    return "Gamble\n" +
                            "Item";
            }
        }
    }

    public static string BGM
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "BGM";
                case Define.GameLanguage.Korean:
                    return "배경음악";
                default:
                    return "BGM";
            }
        }
    }

    public static string SFX
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "SFX";
                case Define.GameLanguage.Korean:
                    return "효과음";
                default:
                    return "SFX";
            }
        }
    }

    public static string ExitToLobby
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Exit To Lobby";
                case Define.GameLanguage.Korean:
                    return "로비로 나가기";
                default:
                    return "Exit To Lobby";
            }
        }
    }

    public static string ResumeToGame
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Resume To Game";
                case Define.GameLanguage.Korean:
                    return "게임으로 돌아가기";
                default:
                    return "Resume To Game";
            }
        }
    }
}
