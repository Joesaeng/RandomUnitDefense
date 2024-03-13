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

    public const int     PlayerUnitHighestLevel = 3;            // �÷��̾� ������ �ְ� ����
    public const float   MonsterRespawnTime = 0.3f;             // ���� ������ �ð�
    public const float   OneStageTime = 15f;                    // 1�������� ���� �ð�
    public const int     OneStageSpawnCount = 50;               // 1���������� ���� ��ȯ Ƚ��
    public const int     MonsterCountForGameOver = 100;         // ���ӿ��������� ���� ����
    public const int     SetUnitCount = 5;                      // ������ ���� ���� ����
    public const int     InitialRuby = 60;                      // ���� ���۽� �ʱ� ��� ��
    public const int     AmountRubyGivenByMonster = 1;          // ���� �Ѹ����� �ִ� ����� ��
    public const int     RubyRequiredOneSpawnPlayerUnit = 20;   // �÷��̾� ������ ��ȯ�ϴµ� �ʿ��� ����� ��

    public const float   UnitAttackAnimLength = 0.417f;         // �÷��̾� ���� ���� �ִϸ��̼��� ����

    public const int     FirstOfMonsterID = 1000;               // ù��° ������ ID
    public const int     FirstOfUnitID = 100;                   // ù��° �÷��̾� ������ ID

    public const int     BaseUpgradeCost = 10;                  // ������ �⺻ ���׷��̵� �ʿ� ��
    // ���� ���׷��̵� �� ������
    public const float   IncreaseAttackRate = 0.95f;
    public const float   IncreaseDebuffDuration = 1.1f;
    public const float   IncreaseDebuffRatio = 1.1f;

    // �ΰ��� ������ �̱� Ȯ�� 
                       //PercentOfCommon             // 0.60 ~ 1
    public const float   PercentOfUnCommon  = 0.60f; // 0.30 ~ 0.60
    public const float   PercentOfRare      = 0.30f; // 0.15 ~ 0.30
    public const float   PercentOfUnique    = 0.15f; // 0.05 ~ 0.15
    public const float   PercentOfLegend    = 0.05f; // 0    ~ 0.05

    public const int     BaseGambleCost  = 10;                  // �⺻ ������ �̱� �ʿ� ��
    public const int     IncreaseGambleCost  = 10;              // �̱� ȸ�� �ʿ� �� ������

    // ������ �ؽ�Ʈ �÷�
    public static readonly Color[] ItemColors =
    {
        Color.white, // 0�� ����
        new Color(30/255f,211/255f,11/255f),
        new Color(27/255f,104/255f,217/255f),
        new Color(169/255f,11/255f,214/255f),
        new Color(214/255f,10/255f,75/255f),
        new Color(214/255f,195/255f,10/255f),
    };


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
