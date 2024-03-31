using Data;
using System.Collections.Generic;
using UnityEngine;

public class CombatScene : BaseScene
{
    UI_CombatScene _ui_scene;

    Define.Map _curMap = Define.Map.Basic;

    // SlotIndex,Unit
    Dictionary<int,Unit> _unitDict;
    
    UnitSlot[] _unitSlots = null;

    UnitNames[] _selectedUnitIds;

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Combat;

        Managers.Game.InitForGameScene(_curMap);
        _unitSlots = GameObject.Find("UnitSlots").gameObject.GetComponentsInChildren<UnitSlot>();

        _unitDict = Managers.Game.UnitDict;

        Managers.Game.OnMoveUnitEvent -= OnMoveUnitBetweenSlots;
        Managers.Game.OnMoveUnitEvent += OnMoveUnitBetweenSlots;

        Managers.Game.OnSpawnButtonClickEvent -= OnSpawnPlayerUnit;
        Managers.Game.OnSpawnButtonClickEvent += OnSpawnPlayerUnit;

        Managers.Game.OnSellAUnit -= OnSellAUnit;
        Managers.Game.OnSellAUnit += OnSellAUnit;

        _selectedUnitIds = Managers.Game.SetUnits;
        
        Managers.InGameItem.Init();
        Managers.UnitStatus.Init();
        Managers.Time.Init();

        #region 오브젝트풀 미리 초기화
        Managers.Resource.Destroy (Managers.Resource.Instantiate("UnitBullet"));
        Managers.Resource.Destroy (Managers.Resource.Instantiate("DamageText"));
        Managers.Resource.Destroy (Managers.Resource.Instantiate("HitEffect_1"));
        Managers.Resource.Destroy (Managers.Resource.Instantiate("HitEffect_2"));
        Managers.Resource.Destroy (Managers.Resource.Instantiate("Unit"));
        Managers.Resource.Destroy (Managers.Resource.Instantiate("Monster"));
        for(int i = 1; i <= ConstantData.HighestStage; ++i)
        {
            string stage = $"{i}";
            string tstage = stage.PadLeft(3, '0');
            Managers.Resource.Destroy(Managers.Resource.Instantiate($"Units/Monster{tstage}"));
        }
        #endregion

        _ui_scene = Managers.UI.ShowSceneUI<UI_CombatScene>();

        Managers.Game.Ruby = ConstantData.InitialRuby;

        Managers.Sound.Play("GameScene",Define.Sound.Bgm);
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
        foreach (Unit unit in _unitDict.Values)
        {
            unit.UnitUpdate();
        }
        if(Managers.Game._dyingMonsters.Count > 0)
        {
            Managers.Game.DyingMonsterDespawn();
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
            if (TryCombineUnit(_unitDict[curSlotIndex], _unitDict[nextSlotIndex]))
            {
                return;
            }
            else
            {
                (_unitDict[curSlotIndex], _unitDict[nextSlotIndex]) 
                    = (_unitDict[nextSlotIndex], _unitDict[curSlotIndex]);

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
    // 랜덤 유닛 선택
    private bool SelectRandomUnit(out int randomSlotIndex, out UnitNames randomId)
    {
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
        randomId = _selectedUnitIds[UnityEngine.Random.Range(0, _selectedUnitIds.Length)];
        randomSlotIndex = randSlotIndex;
        if (randSlotIndex == -1)
        {
            return false;
        }
        return true;
    }
    // 플레이어가 유닛 소환 버튼 클릭 시 호출되는 메서드
    private void OnSpawnPlayerUnit()
    {
        if (Managers.Game.Ruby < ConstantData.RubyRequiredOneSpawnPlayerUnit)
            return;

        if (SelectRandomUnit(out int randSlotIndex, out UnitNames randId) == false)
            return;

        CreatePlayerUnit(randSlotIndex, randId);
        Managers.Game.Ruby -= ConstantData.RubyRequiredOneSpawnPlayerUnit;

        Managers.Sound.Play(Define.SFXNames.SpawnUnit);

        // 운의 룬을 장착중일 때
        if (Managers.UnitStatus.RuneStatus.BaseRuneEffects.TryGetValue(BaseRune.Lucky, out float spawnAnotherUnit))
        {
            if (UnityEngine.Random.value <= spawnAnotherUnit)
            {
                if (SelectRandomUnit(out randSlotIndex, out randId) == false)
                    return;

                Managers.UI.MakeSubItem<UI_NotificationText>().SetText(Define.NotiTexts.LuckyRuneEffect);
                CreatePlayerUnit(randSlotIndex, randId);
            }
        }
    }

    // 플레이어 유닛 생성 메서드
    private void CreatePlayerUnit(int slotIndex, UnitNames id, int level = 1)
    {
        GameObject obj = Managers.Resource.Instantiate("Unit");
        obj.transform.position = GetUnitMovePos(slotIndex);
        Managers.Resource.Instantiate("SpawnEffect", GetUnitMovePos(slotIndex));

        Managers.CompCache.GetOrAddComponentCache(obj, out Unit unit);

        string unitname = $"{Managers.Data.BaseUnitDict[(int)id].baseUnit}";
        unit.Init(slotIndex, id, level, unitname);

        Managers.CompCache.GetOrAddComponentCache(obj, out DraggableUnit draggableUnit);

        draggableUnit.OnDraggableMouseDragEvent -= OnDraggableUnitDragEventReader;
        draggableUnit.OnDraggableMouseDragEvent += OnDraggableUnitDragEventReader;

        draggableUnit.OnDraggableDoubleClickEvent -= OnDraggableUnitDoubleClickEventReader;
        draggableUnit.OnDraggableDoubleClickEvent += OnDraggableUnitDoubleClickEventReader;

        _unitDict.Add(slotIndex, unit);
    }

    // 플레이어가 유닛을 드래그 할 때 호출되는 이벤트
    private void OnDraggableUnitDragEventReader(Unit unit)
    {
        for(int slotIndex = 0;  slotIndex < _unitSlots.Length; ++slotIndex)
        {
            if (unit.SlotIndex == slotIndex)
                continue;
            if (_unitDict.ContainsKey(slotIndex) && AreUnitsComposeable(unit, _unitDict[slotIndex]))
                _unitSlots[slotIndex].SetAbleUpgradeImage();
        }
    }

    // 더블클릭시 씬에 나와있는 유닛들을 돌면서 합성가능한 유닛이 있다면 합성한다
    private void OnDraggableUnitDoubleClickEventReader(Unit unit)
    {
        foreach(Unit nextUnit in _unitDict.Values)
        {
            if (TryCombineUnit(unit, nextUnit))
                return;
        }
        
    }

    // 유닛 합성
    private bool TryCombineUnit(Unit unit, Unit nextUnit)
    {
        if (unit == nextUnit)
            return false;

        if (AreUnitsComposeable(unit, nextUnit))
        {
            UnitNames id = unit.ID;
            int nextLevel = unit.Lv + 1;
            int createSlotIndex = nextUnit.SlotIndex;

            DestroyPlayerUnit(unit.SlotIndex);
            DestroyPlayerUnit(nextUnit.SlotIndex);

            CreatePlayerUnit(createSlotIndex, id, nextLevel);
            Managers.Sound.Play(Define.SFXNames.UnitLevelUp);
            return true;
        }
        return false;
    }

    // 플레이어가 유닛 판매를 시도할 때
    private void OnSellAUnit(Unit unit,int sellCost)
    {
        if (_unitDict.ContainsKey(unit.SlotIndex))
        {
            DestroyPlayerUnit(unit.SlotIndex);
            Managers.Game.Ruby += sellCost;
        }
    }

    // 플레이어 유닛 제거 메서드
    private void DestroyPlayerUnit(int slotIndex)
    {
        if (_unitDict.TryGetValue(slotIndex, out Unit unit))
        {
            unit.gameObject.name = "Unit";

            DraggableUnit draggableUnit = unit.GetComponent<DraggableUnit>();

            draggableUnit.OnDraggableMouseDragEvent -= OnDraggableUnitDragEventReader;
            draggableUnit.OnDraggableDoubleClickEvent -= OnDraggableUnitDoubleClickEventReader;

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

    // 씬 변경 할 때
    public override void Clear()
    {
        for (int i = 0; i < _unitSlots.Length; i++)
        {
            DestroyPlayerUnit(i);
        }

        Managers.Game.UnitAttackRange = null;
        Managers.Game.Ruby = 0;

        Managers.Game.OnMoveUnitEvent -= OnMoveUnitBetweenSlots;
        Managers.Game.OnSpawnButtonClickEvent -= OnSpawnPlayerUnit;
        Managers.Game.OnSellAUnit -= OnSellAUnit;

        _ui_scene.Clear();
    }
}
