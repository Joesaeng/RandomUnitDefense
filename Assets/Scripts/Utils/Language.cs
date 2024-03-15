using Data;
using System.Runtime.InteropServices.WindowsRuntime;
using static Define;
using static Unity.VisualScripting.Icons;

public static class Language
{
    public enum UnitInfos
    {
        AttackType,
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

    public static string GetAttackType(UnitType type)
    {
        int index = (int)type;
        string[] english = {"Common","AOE","Debuffer"};
        string[] korean = {"일반 공격","광역 공격","디버퍼" };
        Define.GameLanguage language = Managers.Game.GameLanguage;
        switch (language)
        {
            case Define.GameLanguage.English:
                return english[index];
            case Define.GameLanguage.Korean:
                return korean[index];
            default:
                return english[index];
        }
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

        if (unitIndex < 0 || unitIndex > englishName.Length)
            return "";
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
            "AttackType",
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
            "공격타입",
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
        get
        {
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

    public static string LanguageButton
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Language";
                case Define.GameLanguage.Korean:
                    return "언어";
                default:
                    return "Language";
            }
        }
    }

    public static string LanguageText
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "English";
                case Define.GameLanguage.Korean:
                    return "한국어";
                default:
                    return "English";
            }
        }
    }

    public static string ResumeOrGoGame
    {
        get
        {
            string ret = Managers.Game.CurrentScene == Define.Scene.Login ? "GoGame" : "Resume";
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return ret;
                case Define.GameLanguage.Korean:
                {
                    if (ret == "GoGame")
                        return "게임 시작";
                    else
                        return "돌아가기";
                }
                default:
                    return ret;
            }
        }
    }

    public static string Combat
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Combat";
                case Define.GameLanguage.Korean:
                    return "전투";
                default:
                    return "Combat";
            }
        }
    }

    public static string Barrack
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Barrack";
                case Define.GameLanguage.Korean:
                    return "배럭";
                default:
                    return "Barrack";
            }
        }
    }

    public static string Shop
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Shop";
                case Define.GameLanguage.Korean:
                    return "상점";
                default:
                    return "Shop";
            }
        }
    }

    public static string StartCombat
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Start Combat";
                case Define.GameLanguage.Korean:
                    return "전투 시작";
                default:
                    return "Start Combat";
            }
        }
    }

    public static string CompleteSetUnits
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "You need to Setting \n Unit In the Barrack";
                case Define.GameLanguage.Korean:
                    return "배럭에서 유닛 설정을 \n 완료해주세요";
                default:
                    return "You need to Setting \n Unit In the Barrack";
            }
        }
    }

    public static string Use
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Use";
                case Define.GameLanguage.Korean:
                    return "사용하기";
                default:
                    return "Use";
            }
        }
    }

    public static string Cancel
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Cancel";
                case Define.GameLanguage.Korean:
                    return "취소";
                default:
                    return "Cancel";
            }
        }
    }

    public static string Lv3Preview
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Lv3 Preview";
                case Define.GameLanguage.Korean:
                    return "Lv3 미리보기";
                default:
                    return "Lv3 Preview";
            }
        }
    }

    public static string AttackPerNSeconds(float second)
    {
        Define.GameLanguage language = Managers.Game.GameLanguage;
        switch (language)
        {
            case Define.GameLanguage.English:
                return $"Atk / {second} Sec";
            case Define.GameLanguage.Korean:
                return $"{second} 초당 공격";
            default:
                return $"Atk / {second} Sec";
        }
    }
}
