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

        // 데이터에 int로 id가 설정되어있는것을 UnitNames로 형변환
        int[] setunits = Managers.Player.Data.setUnits;
        SetUnits = new UnitNames[setunits.Length];
        for (int i = 0; i < setunits.Length; ++i)
            SetUnits[i] = (UnitNames)setunits[i];

        // CombatScene Object 바인딩
        {
            UnitAttackRange = null;
            _monsterSpawnPoint = GameObject.Find("SpawnPoint").transform;
            UnitAttackRange = GameObject.Find("UnitAttackRange").GetOrAddComponent<UnitAttackRange>();
            HpBarPanel = GameObject.Find("UI_HPBarPanel").transform;
        }

        // 이벤트 바인드
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
        // UnitDict 초기화
        if (_unitDict != null)
            _unitDict.Clear();
        else
            _unitDict = new Dictionary<int, Unit>();

        // DyingMonsters 초기화
        if (_dyingMonsters != null)
            DyingMonsterDespawn();
        else
            _dyingMonsters = new Stack<Monster>();

        // Monsters 초기화
        if (_monsters != null)
            _monsters.Clear();
        else
            _monsters = new HashSet<Monster>();

        { // DPS dict 초기화
            if (UnitDPSDict != null)
                UnitDPSDict.Clear();
            else
                UnitDPSDict = new Dictionary<UnitNames, int>();

            for (int i = 0; i < SetUnits.Length; ++i)
                UnitDPSDict.Add(SetUnits[i], 0);

            StartCoroutine(CoDPSUpdate());
        }
    }
    // 이벤트 바인딩 해제
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
    // 플레이어 최고 스테이지 갱신
    private void SetPlayerHighestStage()
    {
        int highestStage = Managers.Player.Data.highestStage;
        Managers.Player.Data.highestStage =
            highestStage > CurStage ? highestStage : CurStage;
    }
    // 다음 스테이지로 가는 이벤트
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
    // 스테이지 클리어시 획득 금화 계산
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
        // 데이터로 정해둔 스테이지의 정보를 불러와서 적용
        if (Managers.Data.StageDict.TryGetValue(CurStage, out var stageData))
        {
            // 현재 스태이지에 나와야 할 몬스터의 수량만큼 소환
            if (CurStageMonsterCount < stageData.monsterSpawnCount)
                SpawnMonster(_monsterSpawnPoint.transform.position);
            // 소환을 다 한 후에 현재 맵에 몬스터가 없으면 스테이지 스킵 활성화
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
    // 플레이어 유닛 이동 메서드
    public void MoveUnitBetweenSlots(int curSlotIndex, int moveSlotIndex)
    {
        Util.CheckTheEventAndCall(OnMoveUnitEvent, curSlotIndex, moveSlotIndex);
    }
    // 현재 프레임에 사망하는 몬스터들을 등록하는 메서드
    public void RegisterDyingMonster(Monster monster)
    {
        _dyingMonsters.Push(monster);
        Ruby += monster.GivenRuny;
    }
    // 현재 프레임에 사망할 몬스터들 디스폰
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
    // 유닛 선택 메서드(공격범위 및 UI_UnitInfo 생성)
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
