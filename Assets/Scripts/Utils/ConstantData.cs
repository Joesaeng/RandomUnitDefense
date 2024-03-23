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


    #region ����������� Contents ������ ������������

    public const int     PlayerUnitHighestLevel = 3;            // �÷��̾� ������ �ְ� ����
    public const float   MonsterRespawnTime = 0.3f;             // ���� ������ �ð�
    public const float   OneStageTime = 15f;                    // 1�������� ���� �ð�
    public const int     OneStageSpawnCount = 50;               // 1���������� ���� ��ȯ Ƚ��
    public const int     MonsterCountForGameOver = 100;         // ���ӿ��������� ���� ����
    public const int     SetUnitCount = 5;                      // ������ ���� ���� ����
    public const int     EquipedRunesCount = 3;                 // ������ �� ���� ���� ����
    public const int     InitialRuby = 6000;                    // ���� ���۽� �ʱ� ��� ��
    public const int     AmountRubyGivenByMonster = 1;          // ���� �Ѹ����� �ִ� ����� ��
    public const int     RubyRequiredOneSpawnPlayerUnit = 20;   // �÷��̾� ������ ��ȯ�ϴµ� �ʿ��� ����� ��

    public const float   UnitAttackAnimLength = 0.417f;         // �÷��̾� ���� ���� �ִϸ��̼��� ����

    public const int     FirstOfMonsterID = 1000;               // ù��° ������ ID
    public const int     FirstOfUnitID = 100;                   // ù��° �÷��̾� ������ ID

    public const int     BaseUpgradeCost = 10;                  // ������ �⺻ ���׷��̵� �ʿ� ��

    public const float   BaseCriticalChance = 0.05f;            // �⺻ ġ��Ÿ Ȯ��
    public const float   BaseCriticalDamageRatio = 1.5f;        // �⺻ ġ��Ÿ ������ 

    // ���� ���׷��̵� �� ������                                   // ���ݷ��� �⺻ ���ݷ� ��ŭ ������
    public const float   IncreaseAttackRate         = 0.95f;    // ���ݼӵ� ���� 5%
    public const float   IncreaseDebuffDuration     = 1.1f;     // ����� ���ӽð� ���� 10%
    public const float   IncreaseDebuffRatio        = 1.1f;     // ����� ���� ���� 10%

    // �ΰ��� ������ �̱� Ȯ�� 
    //                   PercentOfCommon                    0.60 ~ 1        40%
    public const float   PercentOfUnCommonItem  = 0.60f; // 0.30 ~ 0.60     30%
    public const float   PercentOfRareItem      = 0.30f; // 0.15 ~ 0.30     15%
    public const float   PercentOfUniqueItem    = 0.15f; // 0.05 ~ 0.15     10% 
    public const float   PercentOfLegendItem    = 0.05f; // 0    ~ 0.05      5%

    public const int     BaseGambleCost  = 10;                  // �⺻ ������ �̱� �ʿ� ��
    public const int     IncreaseGambleCost  = 10;              // �̱� ȸ�� �ʿ� �� ������

    // �� �̱� Ȯ��
    //                   PercentOfCommonRune                  0.50 ~ 1      50%
    public const float   PercentOfRareRune        = 0.50f; // 0.21 ~ 0.50   29%
    public const float   PercentOfUniqueRune      = 0.21f; // 0.09 ~ 0.21   12%
    public const float   PercentOfLegendRune      = 0.09f; // 0.03 ~ 0.09    6%
    public const float   PercentOfMythRune        = 0.03f; // 0    ~ 0.03    3%

    // �� ��� �� �ΰ� ȿ�� ����
    public static readonly int[] AdditionalEftCountOfRunes = {0,1,2,3,5}; // {Common,Rare,Unique,Legend,Myth}


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

    // �� ��� �ؽ�Ʈ �÷�
    public static readonly Color[] RuneGradeColors =
    {
        new Color(0.78f,0.81f,0.86f),
        new Color(1f,0.78f,0.14f),
        new Color(1f,0.92f,0.34f),
        new Color(0.58f,0.99f,1f),
        new Color(0.91f,0.19f,0.23f),
    };


    #region �ʸ��� �ʿ��� ������ �̵� ��ġ����
    // Basic Map
    public static readonly Vector3[] BasicMapPoint =
    {
        new Vector3 (-3.0f,  4.2f, -1f),
        new Vector3 (-3.0f, -3.4f, -1f),
        new Vector3 (  3.9f, -3.4f, -1f),
        new Vector3 (  3.9f,  4.2f, -1f)
    };
    #endregion

    public const int SelectedUnitPrefabSortOrder = 10;  // LobbyScene ���� ���� �гο� �ִ� �������� SortOrder
    #endregion
}
