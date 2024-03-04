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
                    return "���ּ�ȯ";
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
}
