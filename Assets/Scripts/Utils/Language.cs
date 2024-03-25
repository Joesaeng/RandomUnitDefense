using Data;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using static Define;
using static Unity.VisualScripting.Icons;

public static class Language
{
    #region UnitAndItemDescriptions
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
            "AttackSpeed",
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
            "Decrease Attack Cycle",
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

    #endregion

    #region RuneDescriptions
    public static string GetRuneGradeText(GradeOfRune gradeOfRune)
    {
        Define.GameLanguage language = Managers.Game.GameLanguage;
        string[] englishGrade =
        {
            "Common",
            "Rare",
            "Unique",
            "Legend",
            "Myth"
        };
        string[] koreanGrade =
        {
            "일반",
            "희귀",
            "유니크",
            "전설",
            "신화"
        };
        switch (language)
        {
            case Define.GameLanguage.English:
                return englishGrade[(int)gradeOfRune];
            case Define.GameLanguage.Korean:
                return koreanGrade[(int)gradeOfRune];
            default:
                return englishGrade[(int)gradeOfRune];
        }
    }

    public static string GetRuneNameText(BaseRune baseRune)
    {
        Define.GameLanguage language = Managers.Game.GameLanguage;
        string[] englishRuneName =
        {
            "Knight's Rune",
            "Archer's Rune",
            "Viking's Rune",
            "FireMagician's Rune",
            "Spearman's Rune",
            "Warrior's Rune",
            "PoisonBowMan's Rune",
            "Rich's Rune",
            "Fighter's Rune",
            "Lucky's Rune",
            "Curse's Rune",
        };
        string[] koreanRuneName =
        {
            "기사의 룬",
            "궁수의 룬",
            "바이킹의 룬",
            "파이어매지션의 룬",
            "창기사의 룬",
            "워리어의 룬",
            "포이즌보우맨의 룬",
            "부자의 룬",
            "싸움꾼의 룬",
            "운의 룬",
            "저주의 룬",
        };
        switch (language)
        {
            case Define.GameLanguage.English:
                return englishRuneName[(int)baseRune];
            case Define.GameLanguage.Korean:
                return koreanRuneName[(int)baseRune];
            default:
                return englishRuneName[(int)baseRune];
        }
    }

    public static string GetRuneBaseInfo(BaseRune baseRune, float value)
    {
        string valuetext = $"{MathF.Round(value *100)}%";
        Define.GameLanguage language = Managers.Game.GameLanguage;
        string[] englishRuneBaseInfo =
        {
            $"The Knight's Base Damages Are Increase {valuetext}",
            $"The Archer's Base Damages Are Increase {valuetext}",
            $"The Viking's Base Damages Are Increase {valuetext}",
            $"The FireMagician's Base Damages Are Increase {valuetext}",
            $"The Spearman's Base Damages Are Increase {valuetext}",
            $"The Warrior's Base Damages Are Increase {valuetext}",
            $"The PoisonBowMan's Poison Damages Are Increase {valuetext}",
            $"{valuetext} increase in gold coin acquisition",
            $"Attack speed increases by {valuetext}",
            $"Spawn the unit one more time with a {valuetext} chance.",
            $"Decreases the monster's defense by {valuetext}.",
        };
        string[] koreanRuneBaseInfo =
        {
            $"기사의 기본 피해량이 {valuetext} 증가합니다",
            $"궁수의 기본 피해량이 {valuetext} 증가합니다",
            $"바이킹의 기본 피해량이 {valuetext} 증가합니다",
            $"파이어매지션의 기본 피해량이 {valuetext} 증가합니다",
            $"스피어맨의 기본 피해량이 {valuetext} 증가합니다",
            $"워리어의 기본 피해량이 {valuetext} 증가합니다",
            $"포이즌보우맨의 독 피해량이 {valuetext} 증가합니다",
            $"금화 획득량이 {valuetext} 증가합니다",
            $"공격속도가 {valuetext} 증가합니다",
            $"유닛 소환 시 {valuetext} 확률로 한번 더 소환합니다",
            $"몬스터의 방어력을 {valuetext} 감소시킵니다.",
        };
        switch (language)
        {
            case Define.GameLanguage.English:
                return englishRuneBaseInfo[(int)baseRune];
            case Define.GameLanguage.Korean:
                return koreanRuneBaseInfo[(int)baseRune];
            default:
                return englishRuneBaseInfo[(int)baseRune];
        }
    }

    public static string GetRuneAdditionalEffectText(AdditionalEffectName effectName, float value)
    {
        string valuetext = $"{value}";
        if (effectName != AdditionalEffectName.AddedDamage)
            valuetext = $"{MathF.Round(value * 100)}%";

        Define.GameLanguage language = Managers.Game.GameLanguage;
        string[] englishEffects =
        {
            $"+{valuetext} Increased Damages Of Common Type",
            $"+{valuetext} Increased Damages Of AOE Type",
            $"+{valuetext} Increased Attack Speed Of Common Type",
            $"+{valuetext} Increased Attack Speed Of AOE Type",
            $"+{valuetext} Critical Chance Of Common Type",
            $"+{valuetext} Critical Chance Of AOE Type",
            $"+{valuetext} Add Critical Damage Ratio Of Common Type",
            $"+{valuetext} Add Critical Damage Ratio Of AOE Type",
            $"+{valuetext} Added Damage",
        };
        string[] koreanEffects =
        {
            $"+{valuetext} 단일 공격 타입 데미지",
            $"+{valuetext} 광역 공격 타입 데미지",
            $"+{valuetext} 단일 공격 타입 공격속도",
            $"+{valuetext} 광역 공격 타입 공격속도",
            $"+{valuetext} 단일 공격 타입 치명타 확률",
            $"+{valuetext} 광역 공격 타입 치명타 확률",
            $"+{valuetext} 단일 공격 타입 치명타 피해 비율",
            $"+{valuetext} 광역 공격 타입 치명타 피해 비율",
            $"+{valuetext} 추가 데미지",
        };
        switch (language)
        {
            case Define.GameLanguage.English:
                return englishEffects[(int)effectName];
            case Define.GameLanguage.Korean:
                return koreanEffects[(int)effectName];
            default:
                return englishEffects[(int)effectName];
        }
    }
    #endregion

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
                    return "Spawn";
                case Define.GameLanguage.Korean:
                    return "유닛소환";
                default:
                    return "Spawn";
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

    public static string Rune
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Rune";
                case Define.GameLanguage.Korean:
                    return "룬";
                default:
                    return "Rune";
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

    public static string Clear
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Clear";
                case Define.GameLanguage.Korean:
                    return "해제하기";
                default:
                    return "Clear";
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

    public static string GetTipText()
    {
        Define.GameLanguage language = Managers.Game.GameLanguage;
        string[] koreanTips =
        {
            "광역공격타입의 데미지는 폭발체의 중심에서 100% 피해를 가집니다.",
            "추가데미지는 몬스터의 방어력을 무시합니다.",
            "포이즌보우맨의 독데미지는 몬스터의 방어력을 무시합니다.",
            "매 10 스테이지 마다 방어력이 매우 높은 몬스터가 출현합니다.",
            "게임 언어는 로비의 일시정지 메뉴에서 언제든지 바꿀 수 있습니다.",
            "좋은 하루 보내지 마세요, 최고의 하루를 보내세요!",
            "게임이 재미없을땐 클라이밍을 해보시는건 어떤가요?"
        };
        string[] englishTips =
        {
            "AOEUnits's Attack Damage Is 100% From The Center Of The Explosive.",
            "AddedDamage Is Ignores The Monster's Defense",
            "PoisonBowman's PoisonDamage Ignores The Monster's Defense",
            "Monsters With Very High Defense Appear Every 10 Stages",
            "You Can Change the Game Language At Any Time From The Lobby's Pause Menu",
            "Don't Have A Good Day,Have A Great Day!",
            "If The Game Isn't Fun, Why Not Try Climbing?"
        };

        string retString = "Tip. ";
        int randInt = UnityEngine.Random.Range(0, koreanTips.Length -1);
        switch (language)
        {
            case Define.GameLanguage.English:
                retString += englishTips[randInt];
                break;
            case Define.GameLanguage.Korean:
                retString += koreanTips[randInt];
                break;
            default:
                retString += englishTips[randInt];
                break;
        }
        return retString;
    }

    public static string TenRuneGamblesText
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "10 Rune Gambles \n +1 Unique Rune";
                case Define.GameLanguage.Korean:
                    return "룬 10개 뽑기 \n +1 유니크 룬";
                default:
                    return "10 Rune Gambles \n +1 Unique Rune";
            }
        }
    }

    public static string OneRuneGambleText
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "1 Rune Gamble";
                case Define.GameLanguage.Korean:
                    return "룬 1개 뽑기";
                default:
                    return "1 Rune Gamble";
            }
        }
    }

    public static string GetNotiText(NotiTexts noti)
    {
        Define.GameLanguage language = Managers.Game.GameLanguage;
        string[] englishNoti =
        {
            "Not Enough Gold Coin!",
            "Lucky Rune Effect Activated!",
            "Can't Sell Equiped Rune"
        };
        string[] koreanNoti =
        {
            "금화가 부족합니다!",
            "운의 룬 효과 발동!",
            "장착한 룬은 판매할 수 없습니다."
        };
        switch (language)
        {
            case Define.GameLanguage.English:
                return englishNoti[(int)noti];
            case Define.GameLanguage.Korean:
                return koreanNoti[(int)noti];
            default:
                return englishNoti[(int)noti];
        }
    }

    public static string GameOverStage
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "GameOver Stage";
                case Define.GameLanguage.Korean:
                    return "게임오버 스테이지";
                default:
                    return "GameOver Stage";
            }
        }
    }

    public static string HighestStage
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Highest Stage";
                case Define.GameLanguage.Korean:
                    return "최고 스테이지";
                default:
                    return "Highest Stage";
            }
        }
    }

    public static string KillMonsterCount
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Kill Monster Count";
                case Define.GameLanguage.Korean:
                    return "잡은 몬스터 수";
                default:
                    return "Kill Monster Count";
            }
        }
    }

    public static string EarnedGoldCoin
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Earned Gold Coin";
                case Define.GameLanguage.Korean:
                    return "금화 획득 량";
                default:
                    return "Earned Gold Coin";
            }
        }
    }

    public static string Lobby
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Lobby";
                case Define.GameLanguage.Korean:
                    return "로비";
                default:
                    return "Lobby";
            }
        }
    }

    public static string Retry
    {
        get
        {
            Define.GameLanguage language = Managers.Game.GameLanguage;
            switch (language)
            {
                case Define.GameLanguage.English:
                    return "Retry";
                case Define.GameLanguage.Korean:
                    return "다시하기";
                default:
                    return "Retry";
            }
        }
    }

    public static string GetRuneSortMode(SortModeOfRunes sortMode)
    {
        Define.GameLanguage language = Managers.Game.GameLanguage;
        string[] englishSortMode = {"Type", "Grade"};
        string[] koreanSortMode = {"종류", "등급"};
        switch (language)
        {
            case Define.GameLanguage.English:
                return englishSortMode[(int)sortMode];
            case Define.GameLanguage.Korean:
                return koreanSortMode[(int)sortMode];
            default:
                return englishSortMode[(int)sortMode];
        }
    }
}
