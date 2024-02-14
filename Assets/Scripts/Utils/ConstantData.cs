using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstantData
{
    #region ��� ������
    public const int    PopupUISortOrder = 10;
    public const int    SceneUISortOrder = 9;
    public const int    WorldSpaceUISortOrder = 8;
    #endregion


    // ����������� Contents ������ ������������

    public const int     PlayerUnitHighestLevel = 3;
    public const float   MonsterRespawnTime = 0.3f;
    public const float   OneStageTime = 20f;
    public const int     OneStageSpawnCount = 30;
    public const int     MonsterCountForGameOver = 100;
    public const int     SelectableUnitCount = 5;
    public const int     InitialRuby = 60;
    public const int     AmountRubyGivenByMonster = 2;
    public const int     RubyRequiredOneSpawnPlayerUnit = 20;

    public const float   UnitAttackAnimLength = 0.417f;


    public const int     FirstOfMonsterID = 1000;
    public const int     FirstOfUnitID = 100;

    public const int     BaseUpgradeCost = 10;
    // ���� ���׷��̵� �� ������
    public const float   IncreaseAttackRate = 0.95f;
    public const float   IncreaseDebuffDuration = 1.1f;
    public const float   IncreaseDebuffRatio = 1.1f;

    #region ���� �̵� ����Ʈ ����
    // Basic Map
    public static readonly Vector3[] BasicMapPoint =
    {
        new Vector3 (-3.0f,  4.2f, -1f),
        new Vector3 (-3.0f, -3.4f, -1f),
        new Vector3 (  3.9f, -3.4f, -1f),
        new Vector3 (  3.9f,  4.2f, -1f)
    };
    #endregion
}
