using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstantData
{
    #region 상수 데이터
    public const int    PopupUISortOrder = 10;
    public const int    SceneUISortOrder = 9;
    public const int    WorldSpaceUISortOrder = 8;
    #endregion


    // ▽▽▽▽▽▽▽▽▽▽ Contents 데이터 ▽▽▽▽▽▽▽▽▽▽▽

    public const int     PlayerUnitHighestLevel = 3;
    public const float   MonsterRespawnTime = 0.3f;
    public const float   OneStageTime = 20f;
    public const int     OneStageSpawnCount = 50;
    public const int     MonsterCountForGameOver = 100;
    public const int     SelectableUnitCount = 5;
    public const int     InitialRuby = 60;
    public const int     AmountRubyGivenByMonster = 1;
    public const int     RubyRequiredOneSpawnPlayerUnit = 20;

    public const float   UnitAttackAnimLength = 0.417f;

    public const int     FirstOfMonsterID = 1000;
    public const int     FirstOfUnitID = 100;

    public const int     BaseUpgradeCost = 10;
    // 유닛 업그레이드 당 증가량
    public const float   IncreaseAttackRate = 0.95f;
    public const float   IncreaseDebuffDuration = 1.1f;
    public const float   IncreaseDebuffRatio = 1.1f;

    // 인게임 아이템 뽑기 확률
    public const float   PercentOfUnCommon  = 0.60f;
    public const float   PercentOfRare      = 0.30f;
    public const float   PercentOfUnique    = 0.15f;
    public const float   PercentOfLegend    = 0.05f;

    public const int     FirstOfGambleCost  = 10;
    public const int     IncreaseGambleCost  = 10;

    // 아이템 텍스트 컬러
    public static readonly Color[] ItemColors =
    {
        Color.white, // 0번 없음
        new Color(30/255f,211/255f,11/255f),
        new Color(27/255f,104/255f,217/255f),
        new Color(169/255f,11/255f,214/255f),
        new Color(214/255f,10/255f,75/255f),
        new Color(214/255f,195/255f,10/255f),
    };


    #region 몬스터 이동 포인트 벡터
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
