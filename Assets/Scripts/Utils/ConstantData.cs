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


    #region ▽▽▽▽▽▽▽▽▽▽ Contents 데이터 ▽▽▽▽▽▽▽▽▽▽▽

    public const int     PlayerUnitHighestLevel = 3;            // 플레이어 유닛의 최고 레벨
    public const float   MonsterRespawnTime = 0.3f;             // 몬스터 리스폰 시간
    public const float   OneStageTime = 15f;                    // 1스테이지 진행 시간
    public const int     OneStageSpawnCount = 50;               // 1스테이지의 몬스터 소환 횟수
    public const int     MonsterCountForGameOver = 100;         // 게임오버까지의 몬스터 개수
    public const int     SetUnitCount = 5;                      // 지정된 유닛 설정 개수
    public const int     EquipedRunesCount = 3;                 // 지정된 룬 장착 가능 개수
    public const int     InitialRuby = 6000;                    // 게임 시작시 초기 루비 양
    public const int     AmountRubyGivenByMonster = 1;          // 몬스터 한마리가 주는 루비의 양
    public const int     RubyRequiredOneSpawnPlayerUnit = 20;   // 플레이어 유닛을 소환하는데 필요한 루비의 양

    public const float   UnitAttackAnimLength = 0.417f;         // 플레이어 유닛 공격 애니메이션의 길이

    public const int     FirstOfMonsterID = 1000;               // 첫번째 몬스터의 ID
    public const int     FirstOfUnitID = 100;                   // 첫번째 플레이어 유닛의 ID

    public const int     BaseUpgradeCost = 10;                  // 유닛의 기본 업그레이드 필요 값

    public const float   BaseCriticalChance = 0.05f;            // 기본 치명타 확률
    public const float   BaseCriticalDamageRatio = 1.5f;        // 기본 치명타 데미지 

    // 유닛 업그레이드 당 증가량                                   // 공격력은 기본 공격력 만큼 증가함
    public const float   IncreaseAttackRate         = 0.95f;    // 공격속도 증가 5%
    public const float   IncreaseDebuffDuration     = 1.1f;     // 디버프 지속시간 증가 10%
    public const float   IncreaseDebuffRatio        = 1.1f;     // 디버프 비율 증가 10%

    // 인게임 아이템 뽑기 확률 
    //                   PercentOfCommon                    0.60 ~ 1        40%
    public const float   PercentOfUnCommonItem  = 0.60f; // 0.30 ~ 0.60     30%
    public const float   PercentOfRareItem      = 0.30f; // 0.15 ~ 0.30     15%
    public const float   PercentOfUniqueItem    = 0.15f; // 0.05 ~ 0.15     10% 
    public const float   PercentOfLegendItem    = 0.05f; // 0    ~ 0.05      5%

    public const int     BaseGambleCost  = 10;                  // 기본 아이템 뽑기 필요 값
    public const int     IncreaseGambleCost  = 10;              // 뽑기 회당 필요 값 증가량

    // 룬 뽑기 확률
    //                   PercentOfCommonRune                  0.50 ~ 1      50%
    public const float   PercentOfRareRune        = 0.50f; // 0.21 ~ 0.50   29%
    public const float   PercentOfUniqueRune      = 0.21f; // 0.09 ~ 0.21   12%
    public const float   PercentOfLegendRune      = 0.09f; // 0.03 ~ 0.09    6%
    public const float   PercentOfMythRune        = 0.03f; // 0    ~ 0.03    3%

    // 룬 등급 당 부가 효과 개수
    public static readonly int[] AdditionalEftCountOfRunes = {0,1,2,3,5}; // {Common,Rare,Unique,Legend,Myth}


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

    // 룬 등급 텍스트 컬러
    public static readonly Color[] RuneGradeColors =
    {
        new Color(0.78f,0.81f,0.86f),
        new Color(1f,0.78f,0.14f),
        new Color(1f,0.92f,0.34f),
        new Color(0.58f,0.99f,1f),
        new Color(0.91f,0.19f,0.23f),
    };


    #region 맵마다 필요한 몬스터의 이동 위치벡터
    // Basic Map
    public static readonly Vector3[] BasicMapPoint =
    {
        new Vector3 (-3.0f,  4.2f, -1f),
        new Vector3 (-3.0f, -3.4f, -1f),
        new Vector3 (  3.9f, -3.4f, -1f),
        new Vector3 (  3.9f,  4.2f, -1f)
    };
    #endregion

    public const int SelectedUnitPrefabSortOrder = 10;  // LobbyScene 선택 유닛 패널에 있는 프리펩의 SortOrder
    #endregion
}
