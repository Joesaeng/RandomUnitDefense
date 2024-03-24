using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class CombatScene : BaseScene
{
    UI_CombatScene _ui_scene;

    Define.Map _curMap = Define.Map.Basic;

    // SlotIndex,Unit
    Dictionary<int,Unit> _unitDict = new Dictionary<int,Unit>();
    
    UnitSlot[] _unitSlots = null;

    int[] _selectedUnitIds;

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Combat;

        Managers.Game.InitForGameScene(_curMap);
        _unitSlots = GameObject.Find("UnitSlots").gameObject.GetComponentsInChildren<UnitSlot>();

        Managers.Game.OnMoveUnitEvent -= OnMoveUnitBetweenSlots;
        Managers.Game.OnMoveUnitEvent += OnMoveUnitBetweenSlots;

        Managers.Game.OnSpawnButtonClickEvent -= OnSpawnPlayerUnit;
        Managers.Game.OnSpawnButtonClickEvent += OnSpawnPlayerUnit;

        Managers.Game.OnClickedSellButton -= OnSellAUnit;
        Managers.Game.OnClickedSellButton += OnSellAUnit;

        _selectedUnitIds = Managers.Game.SetUnits;
        
        Managers.InGameItem.Init();
        Managers.UnitStatus.Init();
        Managers.Time.Init();

        #region 오브젝트풀 미리 초기화
        Managers.Resource.Destroy (Managers.Resource.Instantiate
            ("UnitBullet", Managers.Game.UnitBullets));
        Managers.Resource.Destroy (Managers.Resource.Instantiate
             ("DamageText", Managers.Game.DamageTexts));
        Managers.Resource.Destroy (Managers.Resource.Instantiate
                    ("HitEffect_1", Managers.Game.HitEffects));
        Managers.Resource.Destroy (Managers.Resource.Instantiate
                    ("HitEffect_2", Managers.Game.HitEffects));
        #endregion

        _ui_scene = Managers.UI.ShowSceneUI<UI_CombatScene>();

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
        foreach (KeyValuePair<int, Unit> pair in _unitDict)
        {
            pair.Value.UnitUpdate();
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
    // 랜덤 유닛 선택
    private bool SelectRandomUnit(out int randomSlotIndex, out int randomId)
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

        int randSlotIndex;
        int randId;

        if (SelectRandomUnit(out randSlotIndex, out randId) == false)
            return;

        CreatePlayerUnit(randSlotIndex, randId);
        Managers.Game.Ruby -= ConstantData.RubyRequiredOneSpawnPlayerUnit;

        Managers.Sound.Play(Define.SFXNames.SpawnUnit);

        // 운의 룬을 장착중일 때
        float spawnAnotherUnit;
        if(Managers.UnitStatus.RuneStatus.BaseRuneEffects.TryGetValue(BaseRune.Lucky,out spawnAnotherUnit))
        {
            if(UnityEngine.Random.value <= spawnAnotherUnit)
            {
                if (SelectRandomUnit(out randSlotIndex, out randId) == false)
                    return;

                Managers.UI.ShowPopupUI<UI_NotificationText>().SetText(Define.NotiTexts.LuckyRuneEffect);
                CreatePlayerUnit(randSlotIndex, randId);
            }
        }
    }

    // 플레이어 유닛 생성 메서드
    private void CreatePlayerUnit(int slotIndex, int id, int level = 1)
    {
        GameObject obj = Managers.Resource.Instantiate("Unit", newParentName:"Units");
        obj.transform.position = GetUnitMovePos(slotIndex);

        Unit unit = obj.GetOrAddComponent<Unit>();

        string unitname = Managers.Data.BaseUnitDict[id].baseUnit.ToString();
        unit.Init(slotIndex, id, level, unitname);

        unit.GetComponent<DraggableUnit>().OnDraggableMouseDragEvent -= OnDraggableUnitDragEventReader;
        unit.GetComponent<DraggableUnit>().OnDraggableMouseDragEvent += OnDraggableUnitDragEventReader;

        obj.name = $"{unitname} Level [{level}]";

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

    // 플레이어가 유닛을 드래그하고 뗄 때 호출되는 이벤트
    private void OnDraggableUnitMouseUpEventReader()
    {
        foreach (UnitSlot slot in _unitSlots)
            slot.SetBasicImage();
    }

    // 플레이어가 유닛 판매 버튼을 눌렀을 때
    private void OnSellAUnit(Unit unit,int sellCost)
    {
        int slotIndex;
        if(_unitDict.FindKeyByValueInDictionary(unit, out slotIndex))
        {
            DestroyPlayerUnit(slotIndex);
            Managers.Game.Ruby += sellCost;
        }
    }

    // 플레이어 유닛 제거 메서드
    private void DestroyPlayerUnit(int slotIndex)
    {
        Unit unit;
        if (_unitDict.TryGetValue(slotIndex, out unit))
        {
            unit.gameObject.name = "Unit";

            unit.GetComponent<DraggableUnit>().OnDraggableMouseDragEvent -= OnDraggableUnitDragEventReader;
            unit.GetComponent<DraggableUnit>().OnDraggableMouseUpEvent -= OnDraggableUnitMouseUpEventReader;

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
        Managers.Game.OnClickedSellButton -= OnSellAUnit;

        _ui_scene.Clear();
    }
}
