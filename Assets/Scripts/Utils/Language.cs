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
        string[] korean = {"�Ϲ� ����","���� ����","�����" };
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
            "����Ÿ��",
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
            "���ݷ� ����",
            "���� �ֱ� ����",
            "���ݹ��� ����",
            "�������ݹ��� ����",
            "�߰� ������"
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
            "���ݷ� ����",
            "���� �ֱ� ����",
            "���ݹ��� ����",
            "�������ݹ��� ����",
            "�߰� ������"
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
                return $"{second} �ʴ� ����";
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
            "�Ϲ�",
            "���",
            "����ũ",
            "����",
            "��ȭ"
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
            "����� ��",
            "�ü��� ��",
            "����ŷ�� ��",
            "���̾�������� ��",
            "â����� ��",
            "�������� ��",
            "�����𺸿���� ��",
            "������ ��",
            "�ο���� ��",
            "���� ��",
            "������ ��",
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
            $"����� �⺻ ���ط��� {valuetext} �����մϴ�",
            $"�ü��� �⺻ ���ط��� {valuetext} �����մϴ�",
            $"����ŷ�� �⺻ ���ط��� {valuetext} �����մϴ�",
            $"���̾�������� �⺻ ���ط��� {valuetext} �����մϴ�",
            $"���Ǿ���� �⺻ ���ط��� {valuetext} �����մϴ�",
            $"�������� �⺻ ���ط��� {valuetext} �����մϴ�",
            $"�����𺸿���� �� ���ط��� {valuetext} �����մϴ�",
            $"��ȭ ȹ�淮�� {valuetext} �����մϴ�",
            $"���ݼӵ��� {valuetext} �����մϴ�",
            $"���� ��ȯ �� {valuetext} Ȯ���� �ѹ� �� ��ȯ�մϴ�",
            $"������ ������ {valuetext} ���ҽ�ŵ�ϴ�.",
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
            $"+{valuetext} ���� ���� Ÿ�� ������",
            $"+{valuetext} ���� ���� Ÿ�� ������",
            $"+{valuetext} ���� ���� Ÿ�� ���ݼӵ�",
            $"+{valuetext} ���� ���� Ÿ�� ���ݼӵ�",
            $"+{valuetext} ���� ���� Ÿ�� ġ��Ÿ Ȯ��",
            $"+{valuetext} ���� ���� Ÿ�� ġ��Ÿ Ȯ��",
            $"+{valuetext} ���� ���� Ÿ�� ġ��Ÿ ���� ����",
            $"+{valuetext} ���� ���� Ÿ�� ġ��Ÿ ���� ����",
            $"+{valuetext} �߰� ������",
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
                    return "�Ǹ�";
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
                    return "���ּ�ȯ";
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
                    return "������\n" +
                            "�̱�";
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
                    return "�������";
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
                    return "ȿ����";
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
                    return "�κ�� ������";
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
                    return "�������� ���ư���";
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
                    return "���";
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
                    return "�ѱ���";
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
                        return "���� ����";
                    else
                        return "���ư���";
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
                    return "����";
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
                    return "�跰";
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
                    return "��";
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
                    return "���� ����";
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
                    return "�跰���� ���� ������ \n �Ϸ����ּ���";
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
                    return "����ϱ�";
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
                    return "�����ϱ�";
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
                    return "���";
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
                    return "Lv3 �̸�����";
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
            "��������Ÿ���� �������� ����ü�� �߽ɿ��� 100% ���ظ� �����ϴ�.",
            "�߰��������� ������ ������ �����մϴ�.",
            "�����𺸿���� ���������� ������ ������ �����մϴ�.",
            "�� 10 �������� ���� ������ �ſ� ���� ���Ͱ� �����մϴ�.",
            "���� ���� �κ��� �Ͻ����� �޴����� �������� �ٲ� �� �ֽ��ϴ�.",
            "���� �Ϸ� ������ ������, �ְ��� �Ϸ縦 ��������!",
            "������ ��̾����� Ŭ���̹��� �غ��ô°� �����?"
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
                    return "�� 10�� �̱� \n +1 ����ũ ��";
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
                    return "�� 1�� �̱�";
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
            "��ȭ�� �����մϴ�!",
            "���� �� ȿ�� �ߵ�!",
            "������ ���� �Ǹ��� �� �����ϴ�."
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
                    return "���ӿ��� ��������";
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
                    return "�ְ� ��������";
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
                    return "���� ���� ��";
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
                    return "��ȭ ȹ�� ��";
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
                    return "�κ�";
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
                    return "�ٽ��ϱ�";
                default:
                    return "Retry";
            }
        }
    }

    public static string GetRuneSortMode(SortModeOfRunes sortMode)
    {
        Define.GameLanguage language = Managers.Game.GameLanguage;
        string[] englishSortMode = {"Type", "Grade"};
        string[] koreanSortMode = {"����", "���"};
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
