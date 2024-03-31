using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx : MonoBehaviour
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

    Transform _monsterSpawnPoint;

    public Stack<Monster> _dyingMonsters;
    public Stack<Monster> DyingMonsters { get { return _dyingMonsters; } }

    private Dictionary<int,Unit> _unitDict;
    public Dictionary<int, Unit> UnitDict { get { return _unitDict; } }
    WaitForSeconds _asecond = new WaitForSeconds(1f);
    WaitForSeconds _wfsDpsUpdate = new WaitForSeconds(0.1f);

    public Dictionary<UnitNames, int> UnitDPSDict { get; set; }

    public Action OnChangedLanguage;

    public Action<int,int> OnMoveUnitEvent;

    public Action OnSpawnButtonClickEvent;

    public Action<int> OnChangedRuby;

    public Action OnNextStage;

    public Action<UnitNames,int> OnSelectUnit;

    public Action OnUnselectUnit;

    public Action<Unit,int> OnSellAUnit;

    public Action OnDPSChecker;

    public Unit SelectedUnit { get; set; }

    public UnitNames[] SetUnits { get; set; }
    public int[] UpgradeCostOfUnits { get; set; }

    public UnitAttackRange UnitAttackRange { get; set; }

    public int CurStage { get; set; }
    public int CurStageMonsterCount { get; set; }
    public int KillMonsterCount { get; set; }
    public int EarnedGoldCoin { get; set; }
    public bool StageAutoSkip { get; set; } = false;

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

    public Transform HpBarPanel { get; set; }

    public void InitScene(Define.Scene scene)
    {
        CurrentScene = scene;
    }

    public void InitForGameScene(Define.Map map)
    {
        CurStage = 1;
        CurStageMonsterCount = 0;
        KillMonsterCount = 0;
        EarnedGoldCoin = 0;
        StageAutoSkip = false;
        CurMap = map;

        // �����Ϳ� int�� id�� �����Ǿ��ִ°��� UnitNames�� ����ȯ
        int[] setunits = Managers.Player.Data.setUnits;
        SetUnits = new UnitNames[setunits.Length];
        for (int i = 0; i < setunits.Length; ++i)
            SetUnits[i] = (UnitNames)setunits[i];

        // CombatScene Object ���ε�
        {
            UnitAttackRange = null;
            _monsterSpawnPoint = GameObject.Find("SpawnPoint").transform;
            UnitAttackRange = GameObject.Find("UnitAttackRange").GetOrAddComponent<UnitAttackRange>();
            HpBarPanel = GameObject.Find("UI_HPBarPanel").transform;
        }

        // �̺�Ʈ ���ε�
        {
            Managers.Time.OnMonsterRespawnTime -= TheRespawnTime;
            Managers.Time.OnMonsterRespawnTime += TheRespawnTime;

            Managers.Time.OnNextStage -= OnNextStageEvent;
            Managers.Time.OnNextStage += OnNextStageEvent;

            Managers.UnitStatus.OnUnitUpgradeSlot -= OnUnitUpgrade;
            Managers.UnitStatus.OnUnitUpgradeSlot += OnUnitUpgrade;
        }
        UpgradeCostOfUnits = new int[ConstantData.SetUnitCount];
        for (int i = 0; i < Managers.Game.UpgradeCostOfUnits.Length; ++i)
        {
            UpgradeCostOfUnits[i] = ConstantData.BaseUpgradeCost;
        }
        // UnitDict �ʱ�ȭ
        if (_unitDict != null)
            _unitDict.Clear();
        else
            _unitDict = new Dictionary<int, Unit>();

        // DyingMonsters �ʱ�ȭ
        if (_dyingMonsters != null)
            DyingMonsterDespawn();
        else
            _dyingMonsters = new Stack<Monster>();

        // Monsters �ʱ�ȭ
        if (_monsters != null)
            _monsters.Clear();
        else
            _monsters = new HashSet<Monster>();

        { // DPS dict �ʱ�ȭ
            if (UnitDPSDict != null)
                UnitDPSDict.Clear();
            else
                UnitDPSDict = new Dictionary<UnitNames, int>();

            for (int i = 0; i < SetUnits.Length; ++i)
                UnitDPSDict.Add(SetUnits[i], 0);

            StartCoroutine(CoDPSUpdate());
        }
    }
    // �̺�Ʈ ���ε� ����
    public void ClearBindedEvent()
    {
        Managers.Time.OnMonsterRespawnTime -= TheRespawnTime;

        Managers.Time.OnNextStage -= OnNextStageEvent;

        Managers.UnitStatus.OnUnitUpgradeSlot -= OnUnitUpgrade;
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
        UpgradeCostOfUnits[upgradeSlot] = Managers.UnitStatus.UnitUpgradLv[SetUnits[upgradeSlot]] * ConstantData.BaseUpgradeCost;
    }
    // �÷��̾� �ְ� �������� ����
    private void SetPlayerHighestStage()
    {
        int highestStage = Managers.Player.Data.highestStage;
        Managers.Player.Data.highestStage =
            highestStage > CurStage ? highestStage : CurStage;
    }
    // ���� ���������� ���� �̺�Ʈ
    private void OnNextStageEvent()
    {
        EarnedGoldCoin += CalculateEarendGoldCoinForStage();
        CurStage++;
        CurStageMonsterCount = 0;
        if (CurStage > ConstantData.HighestStage)
        {
            CurStage = ConstantData.HighestStage;
            GameOver("Victory");
            return;
        }
        Util.CheckTheEventAndCall(OnNextStage);
    }
    // �������� Ŭ����� ȹ�� ��ȭ ���
    public int CalculateEarendGoldCoinForStage()
    {
        float fCoin = 0f;
        float richRuneValue = 0f;
        Managers.UnitStatus.RuneStatus.BaseRuneEffects.TryGetValue(BaseRune.Rich, out richRuneValue);
        fCoin += CurStage * (richRuneValue + 1f);

        int coin = (int)fCoin;
        return coin;
    }

    private void TheRespawnTime()
    {
        // �����ͷ� ���ص� ���������� ������ �ҷ��ͼ� ����
        if (Managers.Data.StageDict.TryGetValue(CurStage, out var stageData))
        {
            // ���� ���������� ���;� �� ������ ������ŭ ��ȯ
            if (CurStageMonsterCount < stageData.monsterSpawnCount)
                SpawnMonster(_monsterSpawnPoint.transform.position);
            // ��ȯ�� �� �� �Ŀ� ���� �ʿ� ���Ͱ� ������ �������� ��ŵ Ȱ��ȭ
            else if (Monsters.Count <= 0)
            {
                if (StageAutoSkip)
                    Managers.Time.SkipStage();
                else
                {
                    FindObjectOfType<UI_CombatScene>().ActiveSkipButton();
                }

            }
        }
    }

    public void SpawnMonster(Vector3 spawnPoint)
    {
        GameObject monsterObj = Managers.Resource.Instantiate("Monster", spawnPoint);

        Managers.CompCache.GetOrAddComponentCache(monsterObj, out Monster monsterComp);

        monsterComp.Init(CurStage, CurMap);
        if (Managers.Data.StageDict[CurStage].isSpecial)
            monsterObj.transform.localScale = Vector3.one * 1.5f;
        else
            monsterObj.transform.localScale = Vector3.one;
        _monsters.Add(monsterComp);
        CurStageMonsterCount++;

        if (Monsters.Count > ConstantData.MonsterCountForGameOver)
        {
            GameOver("Fail");
            return;
        }
    }

    public void GameOver(string gameoverType)
    {
        SetPlayerHighestStage();
        Managers.Time.GamePause();
        Managers.UI.ShowPopupUI<UI_GameOver>().SetUp(gameoverType);
        Managers.Player.AmountOfGold += EarnedGoldCoin;
        Managers.Player.SaveToJson();
    }

    public void DespawnMonster(Monster monster)
    {
        if (_monsters.Contains(monster))
        {
            _monsters.Remove(monster);
            Managers.Resource.Destroy(monster.gameObject);
        }
    }
    // �÷��̾� ���� �̵� �޼���
    public void MoveUnitBetweenSlots(int curSlotIndex, int moveSlotIndex)
    {
        Util.CheckTheEventAndCall(OnMoveUnitEvent, curSlotIndex, moveSlotIndex);
    }
    // ���� �����ӿ� ����ϴ� ���͵��� ����ϴ� �޼���
    public void RegisterDyingMonster(Monster monster)
    {
        _dyingMonsters.Push(monster);
        Ruby += monster.GivenRuny;
    }
    // ���� �����ӿ� ����� ���͵� ����
    public void DyingMonsterDespawn()
    {
        while (DyingMonsters.Count > 0)
        {
            DespawnMonster(DyingMonsters.Pop());

            KillMonsterCount++;
        }
    }

    public void OnSpawnButtonClicked()
    {
        Util.CheckTheEventAndCall(OnSpawnButtonClickEvent);
    }
    // ���� ���� �޼���(���ݹ��� �� UI_UnitInfo ����)
    public void SelectUnit(Unit unit)
    {
        SelectedUnit = unit;

        Util.CheckTheEventAndCall(OnSelectUnit, unit.ID, unit.Lv);
        UnitAttackRange.ActiveAttackRange(unit);
    }

    public void ClickedSellButton(int sellCost)
    {
        if (SelectedUnit != null)
        {
            if (Util.CheckTheEventAndCall(OnSellAUnit, SelectedUnit, sellCost))
            {
                UnSelectUnit();
            }
        }
    }

    public List<Unit> FindUnitsWithUnitId(UnitNames unitId)
    {
        List<Unit> foundUnits = new List<Unit>();
        foreach (Unit unit in UnitDict.Values)
        {
            if (unit.ID == unitId)
                foundUnits.Add(unit);
        }
        return foundUnits;
    }

    public void SellAllUnits(List<Unit> foundUnits)
    {
        foreach (Unit unit in foundUnits)
        {
            int sellCost = ConstantData.UnitSellingPrices[unit.Lv - 1];
            if (!Util.CheckTheEventAndCall(OnSellAUnit, unit, sellCost))
                Debug.Log("SellAllUnits Event Error");
        }
    }

    public void UnSelectUnit()
    {
        if (SelectedUnit != null)
        {
            SelectedUnit = null;
            Util.CheckTheEventAndCall(OnUnselectUnit);
        }
        if (UnitAttackRange.enabled == true)
        {
            UnitAttackRange.UnActiveAttackRange();
        }
    }

    public void ChangeGameLanguage()
    {
        if (_gameLanguage == Define.GameLanguage.Korean)
            _gameLanguage = Define.GameLanguage.English;
        else
            _gameLanguage = Define.GameLanguage.Korean;
        Managers.Player.Data.gameLanguage = (int)_gameLanguage;
        Util.CheckTheEventAndCall(OnChangedLanguage);
        Managers.Player.SaveToJson();
    }

    public void AddDamagesInDPSDict(UnitNames unit, int damage)
    {
        if (UnitDPSDict.TryGetValue(unit, out _))
        {
            UnitDPSDict[unit] += damage;
            StartCoroutine(CoRemoveDPSInOverSecond(unit, damage));
        }
    }

    IEnumerator CoRemoveDPSInOverSecond(UnitNames unit, int damage)
    {
        yield return _asecond;
        UnitDPSDict[unit] -= damage;
    }

    IEnumerator CoDPSUpdate()
    {
        while(true)
        {
            yield return _wfsDpsUpdate;
            Util.CheckTheEventAndCall(OnDPSChecker);
        }
    }

    public void Clear()
    {
        ClearBindedEvent();
        StopAllCoroutines();
    }
}
