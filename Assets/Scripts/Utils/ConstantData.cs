using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantData
{
    #region ��� ������
    public static readonly int PopupUISortOrder = 10;
    public static readonly int SceneUISortOrder = 9;
    #endregion


    // ����������� Contents ������ ������������

    public static readonly int     PlayerUnitHighestLevel = 3;
    public static readonly float   MonsterRespawnTime = 0.5f;
    public static readonly float   OneStageTime = 15f;
    public static readonly int     OneStageSpawnCount = 20;


    public static readonly int     MonsterIDFirst = 1000;

    #region ���� �̵� ����Ʈ ����
    // Basic Map
    public static readonly Vector3[] BasicMapPoint =
    {
        new Vector3 (-3.55f,  4.7f, -1f),
        new Vector3 (-3.55f, -3.5f, -1f),
        new Vector3 (  4.6f, -3.5f, -1f),
        new Vector3 (  4.6f,  4.7f, -1f)
    };
    #endregion
}
