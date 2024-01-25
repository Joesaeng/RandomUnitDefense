using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantData
{
    #region 상수 데이터
    public static readonly int PopupUISortOrder = 10;
    public static readonly int SceneUISortOrder = 9;
    #endregion


    // ▽▽▽▽▽▽▽▽▽▽ Contents 데이터 ▽▽▽▽▽▽▽▽▽▽▽

    public static readonly int     PlayerUnitHighestLevel = 3;
    public static readonly float   MonsterRespawnTime = 0.5f;
    public static readonly float   OneStageTime = 15f;
    public static readonly int     OneStageSpawnCount = 20;

    public static readonly int     InitialRuby = 60;
    public static readonly int     AmountRubyGivenByMonster = 1;
    public static readonly int     RubyRequiredOneSpawnPlayerUnit = 20;


    public static readonly int     MonsterIDFirst = 1000;

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
