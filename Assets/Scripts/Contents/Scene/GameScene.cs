using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField]
    Transform _monsterSpawnPoint;

    [SerializeField]
    Define.Map _curMap = Define.Map.Basic;

    // SlotIndex,Unit
    Dictionary<int,Unit> _unitDict = new Dictionary<int,Unit>();

    UnitSlot[] _unitSlots = null;

    int[] ids = {100,101,102,103,104,105,106,107,108};

    [SerializeField]
    int[] _selectedUnitIds;

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game;

        Managers.Game.InitForGameScene(_curMap);
        _unitSlots = GameObject.Find("UnitSlots").gameObject.GetComponentsInChildren<UnitSlot>();

        Managers.Game.OnMoveUnitEvent -= OnMoveUnitBetweenSlots;
        Managers.Game.OnMoveUnitEvent += OnMoveUnitBetweenSlots;

        Managers.Game.OnSpawnButtonClickEvent -= OnSpawnPlayerUnit;
        Managers.Game.OnSpawnButtonClickEvent += OnSpawnPlayerUnit;

        Managers.Game.OnClickedSellButton -= OnSellAUnit;
        Managers.Game.OnClickedSellButton += OnSellAUnit;

        Managers.Time.OnMonsterRespawnTime -= TheRespawnTime;
        Managers.Time.OnMonsterRespawnTime += TheRespawnTime;

        #region TEMP // �κ� ������ ������ ������ ���� ������ �����;���

        _selectedUnitIds = new int[ConstantData.SelectableUnitCount];
        HashSet<int> IdSet = new HashSet<int>();
        for (int i = 0; i < _selectedUnitIds.Length; ++i)
        {
            while (true)
            {
                int randId = ids[UnityEngine.Random.Range(0,ids.Length)];
                if(IdSet.Contains(randId))
                    continue;
                IdSet.Add(randId);
                _selectedUnitIds[i] = randId;
                break;
            }
        }
        Managers.Game.SelectedUnitIds = _selectedUnitIds;
        

        #endregion

        Managers.InGameItem.Init();
        Managers.UnitStatus.Init();
        Managers.Time.Init();

        Managers.UI.ShowSceneUI<UI_GameScene>();

        Managers.Sound.Play("GameScene",Define.Sound.Bgm);
    }

    private void Update()
    {
        if (Managers.Time.IsPause)
            return;

        // ������Ʈ ������ ���� �� �ֱ� ������ ���ӽſ���
        // ������Ʈ�� �ʿ��� ��� ������Ʈ���� ������Ʈ�� �����Ѵ�.
        foreach (Monster monster in Managers.Game.Monsters)
        {
            monster.MonsterUpdate();
        }
        foreach (KeyValuePair<int, Unit> pair in _unitDict)
        {
            pair.Value.UnitUpdate();
        }
        if(Managers.Game._dyingMonsters.Count > 0)
        {
            Managers.Game.DyingMonsterDespawn();
        }
    }

    // ������ ���԰� �̵��� ȣ��Ǵ� �޼���
    private void OnMoveUnitBetweenSlots(int curSlotIndex, int nextSlotIndex)
    {
        if (curSlotIndex == nextSlotIndex)
        {
            _unitDict[curSlotIndex].transform.position = GetUnitMovePos(curSlotIndex);
            return;
        }
        // �̵��� ���Կ� ������ ���ٸ�
        if (_unitDict.ContainsKey(nextSlotIndex) == false)
        {
            Unit unit = _unitDict[curSlotIndex];
            _unitDict[nextSlotIndex] = unit.GetComponent<Unit>();
            unit.SlotChange(nextSlotIndex);
            _unitDict.Remove(curSlotIndex);

            _unitDict[nextSlotIndex].transform.position = GetUnitMovePos(nextSlotIndex);
            return;
        }
        // �̵��� ���Կ� �ٸ� ������ �ִٸ� �����Ѵ�
        // �ռ� ������ �����̶�� nextSlot�� �ռ��� ���� ����
        if (_unitDict.ContainsKey(curSlotIndex) && _unitDict.ContainsKey(nextSlotIndex))
        {
            if (AreUnitsComposeable(_unitDict[curSlotIndex], _unitDict[nextSlotIndex]))
            {
                // �ռ��� �����ϴٸ�
                // �� ������ ����, ���� 3�̻� ������ �ƴ϶��. �ռ�����.

                int id = _unitDict[curSlotIndex].GetComponent<Unit>().ID;
                int nextLevel = _unitDict[nextSlotIndex].GetComponent<Unit>().Lv + 1;

                DestroyPlayerUnit(curSlotIndex);
                DestroyPlayerUnit(nextSlotIndex);

                CreatePlayerUnit(nextSlotIndex, id, nextLevel);
            }
            else
            {
                Unit unit = _unitDict[nextSlotIndex];

                _unitDict[nextSlotIndex] = _unitDict[curSlotIndex];
                _unitDict[curSlotIndex] = unit;

                _unitDict[nextSlotIndex].transform.position = GetUnitMovePos(nextSlotIndex);
                _unitDict[curSlotIndex].transform.position = GetUnitMovePos(curSlotIndex);

                _unitDict[nextSlotIndex].SlotChange(nextSlotIndex);
                _unitDict[curSlotIndex].SlotChange(curSlotIndex);
            }
        }
    }

    // ���� �ΰ��� ���� �ռ��� �����Ѱ�?
    private bool AreUnitsComposeable(Unit left, Unit right)
    {
        bool isComposeable = false;

        if (left.ID == right.ID && left.Lv == right.Lv && left.Lv < ConstantData.PlayerUnitHighestLevel)
        {
            isComposeable = true;
        }

        return isComposeable;
    }


    // �÷��̾ ���� ��ȯ ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void OnSpawnPlayerUnit()
    {
        if (Managers.Game.Ruby < ConstantData.RubyRequiredOneSpawnPlayerUnit)
            return;
        List<int> randIndexList = new List<int>();
        int randSlotIndex = -1;
        for (int i = 0; i < _unitSlots.Length; ++i)
        {
            if (_unitDict.ContainsKey(i) == true)
                continue;

            randIndexList.Add(i);
        }
        if (randIndexList.Count > 0)
        {
            int idx = UnityEngine.Random.Range(0, randIndexList.Count);
            randSlotIndex = randIndexList[idx];
        }
        if (randSlotIndex == -1)
        {
            Debug.Log("unitSlots is Full!");
            return;
        }
        int randId = _selectedUnitIds[UnityEngine.Random.Range(0, _selectedUnitIds.Length)];

        CreatePlayerUnit(randSlotIndex, randId);
        Managers.Game.Ruby -= ConstantData.RubyRequiredOneSpawnPlayerUnit;

        Managers.Sound.Play("SpawnUnit");
    }

    // �÷��̾� ���� ���� �޼���
    private void CreatePlayerUnit(int slotIndex, int id, int level = 1)
    {
        GameObject obj = Managers.Game.Spawn("Unit",newParentName:"Units");

        obj.transform.position = GetUnitMovePos(slotIndex);

        Unit unit = obj.GetOrAddComponent<Unit>();

        string unitname = Managers.Data.BaseUnitDict[id].baseUnit.ToString();
        unit.Init(slotIndex, id, level, unitname);

        obj.name = $"{unitname} Level [{level}]";

        _unitDict.Add(slotIndex, unit);
    }

    private void OnSellAUnit(Unit unit,int sellCost)
    {
        int slotIndex;
        if(_unitDict.FindKeyByValueInDictionary(unit, out slotIndex))
        {
            DestroyPlayerUnit(slotIndex);
            Managers.Game.Ruby += sellCost;
        }
    }

    // �÷��̾� ���� ���� �޼���
    private void DestroyPlayerUnit(int slotIndex)
    {
        Unit unit;
        if (_unitDict.TryGetValue(slotIndex, out unit))
        {
            unit.gameObject.name = "Unit";

            Managers.Resource.Destroy(unit.gameObject);
            _unitDict.Remove(slotIndex);
        }
    }

    // �÷��̾� ������ �̵��� ��ġ�� position�� ����ִ� �޼���
    private Vector3 GetUnitMovePos(int slotIndex)
    {
        // ������ ��ġ�� UnitSlot ������Ʈ ��� ���ϼ��� ������
        // Unit�� ���콺 �̺�Ʈ�� ����������
        // ������ �ȵǴ� ����? ����
        // Unit�� ���������� ī�޶������� 1��ŭ �̵��ϰ� �Ͽ� �ذ�
        return _unitSlots[slotIndex].transform.position + Vector3.back;
    }

    public override void Clear()
    {
        for (int i = 0; i < _unitSlots.Length; i++)
        {
            DestroyPlayerUnit(i);
        }

        Managers.Game.UnitAttackRange = null;
        Managers.Game.Ruby = 0;

        Managers.Game.OnMoveUnitEvent -= OnMoveUnitBetweenSlots;

        Managers.Time.OnMonsterRespawnTime -= TheRespawnTime;

        Managers.Game.OnSpawnButtonClickEvent -= OnSpawnPlayerUnit;

        Managers.Game.OnClickedSellButton -= OnSellAUnit;
    }

    // ���͸� �������� �ð��� �Ǿ���! �� �̺�Ʈ
    private void TheRespawnTime()
    {
        if (Managers.Game.CurStageMonsterCount < ConstantData.OneStageSpawnCount)
            SpawnMonster();
    }

    // ���� ���� ��ȯ �޼���
    private void SpawnMonster()
    {
        Managers.Game.Spawn("Monster",newParentName:"Monsters").transform.position 
            = _monsterSpawnPoint.position;
    }
}
