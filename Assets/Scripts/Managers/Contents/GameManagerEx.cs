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
    public Define.GameLanguage GameLanguage 
    { 
        get { return _gameLanguage; } 
        set 
        {
            _gameLanguage = value;
            Util.CheckTheEventAndCall(OnChangedLanguage);
        }
    }

    public Define.Scene CurrentScene { get; set; } = Define.Scene.Unknown;

    public Define.Map CurMap { get; set; } = Define.Map.Basic;

    HashSet<Monster> _monsters;
    public HashSet<Monster> Monsters { get { return _monsters; } }

    public Stack<GameObject> _dyingMonsters;
    public Stack<GameObject> DyingMonsters { get { return _dyingMonsters; } }

    public Action OnChangedLanguage;

    public Action<int,int> OnMoveUnitEvent;

    public Action OnSpawnButtonClickEvent;

    public Action<int> OnChangedRuby;

    public Action OnNextStage;
    public Action<Unit,int> OnClickedSellButton;

    public GameObject SelectedUnit { get; set; }
    public UI_UnitInfo SelectUnitInfoUI { get; set; }

    public int[] SetUnits { get; set; }
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
            Util.CheckTheEventAndCall(OnChangedRuby, Ruby);
        }
    }

    public void InitScene(Define.Scene scene)
    {
        CurrentScene = scene;
    }

    public void InitForGameScene(Define.Map map)
    {
        CurStage = 1;
        CurStageMonsterCount = 0;

        CurMap = map;

        SetUnits = new int[ConstantData.SetUnitCount];
        // 플레이어 데이터 매니저에서 선택된 유닛을 가져와서 넣어줘야함
        // SelectedUnitIds = PlayerManager.SetedUnits;

        UnitAttackRange = null;
        UnitAttackRange = GameObject.Find("UnitAttackRange").GetOrAddComponent<UnitAttackRange>();

        Managers.Game.Ruby = ConstantData.InitialRuby;

        Managers.Time.OnNextStage.AddEvent(OnNextStageEvent);
        Managers.UnitStatus.OnUnitUpgradeSlot.AddEvent(OnUnitUpgrade);

        UpgradeCostOfUnits = new int[ConstantData.SetUnitCount];
        for (int i = 0; i < Managers.Game.UpgradeCostOfUnits.Length; ++i)
        {
            UpgradeCostOfUnits[i] = ConstantData.BaseUpgradeCost;
        }

        if (_dyingMonsters != null)
            DyingMonsterDespawn();
        else
            _dyingMonsters = new Stack<GameObject>();
        if (_monsters != null)
            _monsters.Clear();
        else
            _monsters = new HashSet<Monster>();
    }

    public bool CanUnitUpgrade(int upgradeSlot)
    {
        if (Ruby >= UpgradeCostOfUnits[upgradeSlot])
        {
            Ruby -= UpgradeCostOfUnits[upgradeSlot];
            return true;
        }
        return false;
    }

    private void OnUnitUpgrade(int upgradeSlot)
    {
        UpgradeCostOfUnits[upgradeSlot] = Managers.UnitStatus.UnitUpgradLv[(UnitNames)SetUnits[upgradeSlot]] * ConstantData.BaseUpgradeCost;
    }

    private void OnNextStageEvent()
    {
        CurStage++;
        CurStageMonsterCount = 0;
        Util.CheckTheEventAndCall(OnNextStage);
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
        if (go.TryGetComponent(out Monster monster) == true)
        {
            if (_monsters.Contains(monster))
            {
                _monsters.Remove(monster);
                Managers.Resource.Destroy(go);
            }
        }


    }

    // 플레이어 유닛 이동 메서드
    public void MoveUnitBetweenSlots(int curSlotIndex, int moveSlotIndex)
    {
        Util.CheckTheEventAndCall(OnMoveUnitEvent, curSlotIndex, moveSlotIndex);
    }
    // 현재 프레임에 사망하는 몬스터들을 등록하는 메서드
    public void RegisterDyingMonster(GameObject monster)
    {
        _dyingMonsters.Push(monster);
    }
    // 현재 프레임에 사망할 몬스터들 디스폰
    public void DyingMonsterDespawn()
    {
        while (DyingMonsters.Count > 0)
        {
            Despawn(DyingMonsters.Pop());
            Ruby += ConstantData.AmountRubyGivenByMonster;
        }
    }

    public void OnSpawnButtonClicked()
    {
        Util.CheckTheEventAndCall(OnSpawnButtonClickEvent);
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
            if (Util.CheckTheEventAndCall(OnClickedSellButton, SelectedUnit.GetComponent<Unit>(), sellCost))
            {
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

    public void ChangeGameLanguage()
    {
        if (_gameLanguage == Define.GameLanguage.Korean)
            _gameLanguage = Define.GameLanguage.English;
        else
            _gameLanguage = Define.GameLanguage.Korean;
        Util.CheckTheEventAndCall(OnChangedLanguage);
    }
}
