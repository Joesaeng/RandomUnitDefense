using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField]
    Transform _monsterSpawnPoint;

    [SerializeField]
    Define.Map _curMap = Define.Map.Basic;

    Dictionary<int,Unit> _unitDict = new Dictionary<int,Unit>();

    UnitSlot[] _unitSlots = null;

    int[] ids = {100,101,102,103,104,105,106,107,108};

    [SerializeField]
    int[] _selectedUnitIds;

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game;
        Managers.Game.CurMap = _curMap;

        _unitSlots = GameObject.Find("UnitSlots").gameObject.GetComponentsInChildren<UnitSlot>();

        Managers.Game._unitAttackRange = GameObject.Find("UnitAttackRange").GetOrAddComponent<UnitAttackRange>();
        Managers.Game.Ruby = ConstantData.InitialRuby;

        Managers.Game.OnMoveUnitEvent -= OnMoveUnitBetweenSlots;
        Managers.Game.OnMoveUnitEvent += OnMoveUnitBetweenSlots;

        Managers.Game.OnSpawnButtonClickEvent -= OnSpawnPlayerUnit;
        Managers.Game.OnSpawnButtonClickEvent += OnSpawnPlayerUnit;

        Managers.Time.OnMonsterRespawnTime -= TheRespawnTime;
        Managers.Time.OnMonsterRespawnTime += TheRespawnTime;

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
        Managers.Game._selectedUnitIds = _selectedUnitIds;
        Managers.Game._upgradeCostOfUnits = new int[ConstantData.SelectableUnitCount];
        for(int i = 0; i < Managers.Game._upgradeCostOfUnits.Length; ++i)
        {
            Managers.Game._upgradeCostOfUnits[i] = ConstantData.BaseUpgradeCost;
        }
        Managers.UnitStatus.Init();
        Managers.UI.ShowSceneUI<UI_GameScene>();
    }

    private void Update()
    {
        if (Managers.Time.IsPause)
            return;

        // 업데이트 순서가 꼬일 수 있기 때문에 게임신에서
        // 업데이트가 필요한 모든 오브젝트들의 업데이트를 관리한다.
        foreach (Monster monster in Managers.Game.Monsters)
        {
            monster.MonsterUpdate();
        }
        foreach (KeyValuePair<int, Unit> pair in _unitDict)
        {
            pair.Value.UnitUpdate();
        }
        while (Managers.Game._dyingMonsters.Count > 0)
        {
            GameObject dyingMonster = Managers.Game._dyingMonsters.Pop();
            DestroyMonster(dyingMonster);
            // TODO
            // 몬스터 카운트 줄이기, 몬스터 처치 재화 획득 등 몬스터가 사망할 때 생기는 이벤트들 실행
            Managers.Game.Ruby += ConstantData.AmountRubyGivenByMonster;
        }
    }

    // 유닛의 슬롯간 이동시 호출되는 메서드
    private void OnMoveUnitBetweenSlots(int curSlotIndex, int nextSlotIndex)
    {
        if (curSlotIndex == nextSlotIndex)
        {
            _unitDict[curSlotIndex].transform.position = GetUnitMovePos(curSlotIndex);
            return;
        }
        // 이동할 슬롯에 유닛이 없다면
        if (_unitDict.ContainsKey(nextSlotIndex) == false)
        {
            Unit unit = _unitDict[curSlotIndex];
            _unitDict[nextSlotIndex] = unit.GetComponent<Unit>();
            unit.SlotChange(nextSlotIndex);
            _unitDict.Remove(curSlotIndex);

            _unitDict[nextSlotIndex].transform.position = GetUnitMovePos(nextSlotIndex);
            return;
        }
        // 이동할 슬롯에 다른 유닛이 있다면 스왑한다
        // 합성 가능한 유닛이라면 nextSlot에 합성된 유닛 생성
        if (_unitDict.ContainsKey(curSlotIndex) && _unitDict.ContainsKey(nextSlotIndex))
        {
            if (AreUnitsComposeable(_unitDict[curSlotIndex], _unitDict[nextSlotIndex]))
            {
                // 합성이 가능하다면
                // 두 유닛이 같고, 레벨 3이상 유닛이 아니라면. 합성가능.

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

    // 유닛 두개가 같고 합성이 가능한가?
    private bool AreUnitsComposeable(Unit left, Unit right)
    {
        bool isComposeable = false;

        if (left.ID == right.ID && left.Lv == right.Lv && left.Lv < ConstantData.PlayerUnitHighestLevel)
        {
            isComposeable = true;
        }

        return isComposeable;
    }


    // 플레이어가 유닛 소환 버튼 클릭 시 호출되는 메서드
    private void OnSpawnPlayerUnit()
    {
        if (Managers.Game.Ruby < ConstantData.RubyRequiredOneSpawnPlayerUnit)
        {
            Debug.Log("루비가 부족합니다!");
            return;
        }
        List<int> randIndexList = new List<int>();
        int randSlotIndex = -1;
        for (int i = 0; i < _unitSlots.Length; ++i)
        {
            // 0부터 unitSlots.Length 까지의 숫자 중에서, 이미 선택된 숫자는 제외한다.
            //int rand = UnityEngine.Random.Range(0, _unitSlots.Length);
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

        // randId는 로비에서 등록한 유닛들의 Id를 가져와서 n개중 1개를 선택하는 방식으로 한다.


        CreatePlayerUnit(randSlotIndex, randId);
        Managers.Game.Ruby -= ConstantData.RubyRequiredOneSpawnPlayerUnit;
    }

    // 플레이어 유닛 생성 메서드
    private void CreatePlayerUnit(int slotIndex, int id, int level = 1)
    {
        GameObject obj = Managers.Game.Spawn("Unit",newParentName:"Units");

        obj.transform.position = GetUnitMovePos(slotIndex);

        Unit unit = obj.GetOrAddComponent<Unit>();
        unit.Init(slotIndex, id, level);

        obj.name = $"{Managers.Data.BaseUnitDict[id].baseUnit} Level [{level}]";

        _unitDict.Add(slotIndex, unit);
    }

    // 플레이어 유닛 제거 메서드
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

    // 플레이어 유닛이 이동할 위치의 position을 뱉어주는 메서드
    private Vector3 GetUnitMovePos(int slotIndex)
    {
        // 유닛의 위치가 UnitSlot 오브젝트 등과 동일선상에 있으니
        // Unit의 마우스 이벤트가 간헐적으로
        // 동작이 안되는 버그? 있음
        // Unit을 강제적으로 카메라쪽으로 1만큼 이동하게 하여 해결
        return _unitSlots[slotIndex].transform.position + Vector3.back;
    }

    public override void Clear()
    {
        for (int i = 0; i < _unitSlots.Length; i++)
        {
            DestroyPlayerUnit(i);
        }

        Managers.Game._unitAttackRange = null;
        Managers.Game.Ruby = 0;

        Managers.Game.OnMoveUnitEvent -= OnMoveUnitBetweenSlots;

        Managers.Time.OnMonsterRespawnTime -= TheRespawnTime;

        Managers.Game.OnSpawnButtonClickEvent -= OnSpawnPlayerUnit;
    }

    // 몬스터를 리스폰할 시간이 되었다! 는 이벤트
    private void TheRespawnTime()
    {
        if (Managers.Game.CurStageMonsterCount < ConstantData.OneStageSpawnCount)
            SpawnMonster(Managers.Game.CurStage);
    }

    // 몬스터 유닛 소환 메서드
    private void SpawnMonster(int stageNum)
    {
        GameObject monster = Managers.Game.Spawn("Monster",newParentName:"Monsters");

        monster.transform.position = _monsterSpawnPoint.position;
        // monsters의 Count 정보를 가지고 GameOver를 결정함.
        // if(monsters.Count >= gameOverCount)
        //    GameOver(); 뭐 이런거?

    }

    // 몬스터 유닛 제거 메서드
    private void DestroyMonster(GameObject monster)
    {
        Managers.Game.Despawn(monster);
    }
}
