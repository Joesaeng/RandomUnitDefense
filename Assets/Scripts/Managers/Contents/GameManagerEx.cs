using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class GameManagerEx
{
    private Define.GameLanguage _gameLanguage = Define.GameLanguage.Korean;

    public Define.GameLanguage GameLanguage => _gameLanguage;

    HashSet<GameObject> _monsters = new HashSet<GameObject>();
    public HashSet<GameObject> Monsters { get { return _monsters; } }

    public Stack<GameObject> _dyingMonsters = new Stack<GameObject>();

    public Action<int> OnSpawnEvent;

    public Action<int,int> OnMoveUnitEvent;

    public GameObject _selectedUnit;
    public UI_UnitInfo _selectUnitInfoUI;

    public UnitAttackRange _unitAttackRange;

    public void Init()
    {
        _unitAttackRange = GameObject.Find("UnitAttackRange").GetOrAddComponent<UnitAttackRange>();
    }

    public GameObject Spawn(string path, Transform parent = null, string newParentName = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent, newParentName);

        if(path == "Monster")
            _monsters.Add(go);

        return go;
    }

    public void Despawn(GameObject go)
    {
        if (_monsters.Contains(go))
        {
            _monsters.Remove(go);
            if (OnSpawnEvent != null)
                OnSpawnEvent.Invoke(-1);
        }

        Managers.Resource.Destroy(go);
    }

    // 플레이어 유닛 이동 메서드
    public void MoveUnitBetweenSlots(int curSlotIndex, int moveSlotIndex)
    {
        if (OnMoveUnitEvent != null)
            OnMoveUnitEvent.Invoke(curSlotIndex, moveSlotIndex);
    }

    // 현재 프레임에 사망하는 몬스터들을 등록하는 메서드
    public void RegisterDyingMonster(GameObject monster)
    {
        _dyingMonsters.Push(monster);
    }

    // 유닛 선택 메서드(공격범위 및 UI_UnitInfo 생성)
    public void SelectUnit(GameObject unit)
    {
        if(_selectedUnit != null && _selectUnitInfoUI != null)
        {
            Managers.UI.CloseWorldSpaceUI(_selectUnitInfoUI.gameObject);
        }

        _selectedUnit = unit;
        _selectUnitInfoUI = Managers.UI.MakeWorldSpaceUI<UI_UnitInfo>(unit.transform);
        _unitAttackRange.ActiveAttackRange(unit);
    }

    public void UnSelectUnit()
    {
        if (_selectedUnit != null && _selectUnitInfoUI != null)
        {
            Managers.UI.CloseWorldSpaceUI(_selectUnitInfoUI.gameObject);
            _selectedUnit = null;
            if(_unitAttackRange.enabled == true)
            {
                _unitAttackRange.UnActiveAttackRange();
            }
        }
    }
}
