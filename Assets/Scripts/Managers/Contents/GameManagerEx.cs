using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

public class GameManagerEx
{
    private Define.GameLanguage _gameLanguage = Define.GameLanguage.Korean;
    public Define.GameLanguage GameLanguage { get { return _gameLanguage; } }

    public Define.Map CurMap { get; set; } = Define.Map.Basic;

    HashSet<Monster> _monsters = new HashSet<Monster>();
    public HashSet<Monster> Monsters { get { return _monsters; } }

    public Stack<GameObject> _dyingMonsters = new Stack<GameObject>();
    public Stack<GameObject> DyingMonsters { get { return _dyingMonsters; } }

    public Stack<DamageText> _damageTextPool = new Stack<DamageText>();
    public Stack<DamageText> DamageTextPool {get{return _damageTextPool;}}

    public Action<int> OnSpawnEvent;

    public Action<int,int> OnMoveUnitEvent;

    public Action OnSpawnButtonClickEvent;

    public Action<int> OnChangedRuby;

    public Action OnNextStage;
    public Action<Unit,int> OnClickedSellButton;

    public GameObject SelectedUnit { get; set; }
    public UI_UnitInfo SelectUnitInfoUI { get; set; }

    public int[] SelectedUnitIds { get; set; }
    public int[] UpgradeCostOfUnits { get; set; }

    public UnitAttackRange UnitAttackRange { get; set; }

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

        Managers.UnitStatus.OnUnitUpgradeSlot -= OnUnitUpgrade;
        Managers.UnitStatus.OnUnitUpgradeSlot += OnUnitUpgrade;
    }

    public bool CanUnitUpgrade(int upgradeSlot)
    {
        if(Ruby >= UpgradeCostOfUnits[upgradeSlot])
        {
            Ruby -= UpgradeCostOfUnits[upgradeSlot];
            return true;
        }
        return false;
    }

    private void OnUnitUpgrade(int upgradeSlot)
    {
        UpgradeCostOfUnits[upgradeSlot] = Managers.UnitStatus.UnitUpgradLv[(UnitNames)SelectedUnitIds[upgradeSlot]] * ConstantData.BaseUpgradeCost;
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
            Monster monster = go.GetOrAddComponent<Monster>();
            monster.Init(CurStage, CurMap);
            _monsters.Add(monster);
            CurStageMonsterCount++;
        }

        return go;
    }

    public void Despawn(GameObject go)
    {
        Monster monster;
        if (go.TryGetComponent(out monster) == true)
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
        if (SelectedUnit != null && SelectUnitInfoUI != null)
        {
            Managers.UI.CloseWorldSpaceUI(SelectUnitInfoUI.gameObject);
        }

        SelectedUnit = unit;
        SelectUnitInfoUI = Managers.UI.MakeWorldSpaceUI<UI_UnitInfo>(unit.transform);
        UnitAttackRange.ActiveAttackRange(unit);
    }

    public void ClickedSellButton(int sellCost)
    {
        if (SelectedUnit != null && SelectUnitInfoUI != null)
        {
            if (OnClickedSellButton != null)
            {
                OnClickedSellButton.Invoke(SelectedUnit.GetComponent<Unit>(),sellCost);
                UnSelectUnit();
            }
        }
    }

    public void UnSelectUnit()
    {
        if (SelectedUnit != null && SelectUnitInfoUI != null)
        {
            Managers.UI.CloseWorldSpaceUI(SelectUnitInfoUI.gameObject);
            SelectedUnit = null;
            if (UnitAttackRange.enabled == true)
            {
                UnitAttackRange.UnActiveAttackRange();
            }
        }
    }
}
