using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManagerEx
{
    private Define.GameLanguage _gameLanguage = Define.GameLanguage.Korean;

    public Define.GameLanguage GameLanguage => _gameLanguage;

    HashSet<Monster> _monsters = new HashSet<Monster>();
    public HashSet<Monster> Monsters { get { return _monsters; } }

    public Stack<GameObject> _dyingMonsters = new Stack<GameObject>();

    public Action<int> OnSpawnEvent;

    public Action<int,int> OnMoveUnitEvent;

    public Action OnSpawnButtonClickEvent;

    public Action<int> OnChangedRuby;

    public Action OnNextStage;

    public GameObject _selectedUnit;
    public UI_UnitInfo _selectUnitInfoUI;

    public UnitAttackRange _unitAttackRange;

    public int CurStage { get; set; }
    public int CurStageMonsterCount { get; set; }

    private int _ruby;
    public int Ruby
    {
        get { return _ruby; }
        set 
        { 
            _ruby = value;
            if (OnChangedRuby != null)
                OnChangedRuby.Invoke(Ruby);
        }
    }

    public void Init()
    {
        CurStage = 1;

        Managers.Time.OnNextStage -= OnNextStageEvent;
        Managers.Time.OnNextStage += OnNextStageEvent;
    }

    private void OnNextStageEvent()
    {
        CurStage++;
        CurStageMonsterCount = 0;
        if (OnNextStage != null)
            OnNextStage.Invoke();
    }

    public GameObject Spawn(string path, Transform parent = null, string newParentName = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent, newParentName);

        if (path == "Monster")
        {
            _monsters.Add(go.GetOrAddComponent<Monster>());
            CurStageMonsterCount++;
        }

        return go;
    }

    public void Despawn(GameObject go)
    {
        Monster monster;
        if (go.TryGetComponent<Monster>(out monster) == true)
        {
            if (_monsters.Contains(monster))
            {
                _monsters.Remove(monster);
                if (OnSpawnEvent != null)
                    OnSpawnEvent.Invoke(-1);
                Managers.Resource.Destroy(go);
            }
        }


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

    public void OnSpawnButtonClicked()
    {
        if(OnSpawnButtonClickEvent != null)
            OnSpawnButtonClickEvent.Invoke();
    }

    // 유닛 선택 메서드(공격범위 및 UI_UnitInfo 생성)
    public void SelectUnit(GameObject unit)
    {
        if (_selectedUnit != null && _selectUnitInfoUI != null)
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
            if (_unitAttackRange.enabled == true)
            {
                _unitAttackRange.UnActiveAttackRange();
            }
        }
    }
}
