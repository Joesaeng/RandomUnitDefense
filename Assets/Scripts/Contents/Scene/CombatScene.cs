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

        #region ������ƮǮ �̸� �ʱ�ȭ
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
    // ���� ���� ����
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
    // �÷��̾ ���� ��ȯ ��ư Ŭ�� �� ȣ��Ǵ� �޼���
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

        // ���� ���� �������� ��
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

    // �÷��̾� ���� ���� �޼���
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

    // �÷��̾ ������ �巡�� �� �� ȣ��Ǵ� �̺�Ʈ
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

    // �÷��̾ ������ �巡���ϰ� �� �� ȣ��Ǵ� �̺�Ʈ
    private void OnDraggableUnitMouseUpEventReader()
    {
        foreach (UnitSlot slot in _unitSlots)
            slot.SetBasicImage();
    }

    // �÷��̾ ���� �Ǹ� ��ư�� ������ ��
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

            unit.GetComponent<DraggableUnit>().OnDraggableMouseDragEvent -= OnDraggableUnitDragEventReader;
            unit.GetComponent<DraggableUnit>().OnDraggableMouseUpEvent -= OnDraggableUnitMouseUpEventReader;

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

    // �� ���� �� ��
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
