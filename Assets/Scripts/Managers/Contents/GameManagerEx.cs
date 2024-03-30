using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Scripting;

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
    WaitForSeconds _aSecond = new WaitForSeconds(1f);

    public Dictionary<UnitNames,Queue<int>> UnitDPSDict { get; set; }

    public Action OnChangedLanguage;

    public Action<int,int> OnMoveUnitEvent;

    public Action OnSpawnButtonClickEvent;

    public Action<int> OnChangedRuby;

    public Action OnNextStage;
    public Action<Unit,int> OnSellAUnit;

    public Action OnDPSChecker;

    public GameObject SelectedUnit { get; set; }
    public UI_UnitInfo SelectUnitInfoUI { get; set; }

    public UnitNames[] SetUnits { get; set; }
    public int[] UpgradeCostOfUnits { get; set; }

    public UnitAttackRange UnitAttackRange { get; set; }

    public int CurStage { get; set; }
    public int CurStageMonsterCount { get; set; }
    public int KillMonsterCount { get; set; }
    public int EarnedGoldCoin { get; set; }

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
    public Transform DamageTexts { get; set; }
    public Transform HitEffects { get; set; }
    public Transform UnitBullets { get; set; }

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
            DamageTexts = GameObject.Find("DamageTexts").transform;
            HitEffects = GameObject.Find("HitEffects").transform;
            UnitBullets = GameObject.Find("UnitBullets").transform;
        }

        Managers.Game.Ruby = ConstantData.InitialRuby;

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
                UnitDPSDict = new Dictionary<UnitNames,Queue<int>>();

            for (int i = 0; i < SetUnits.Length; ++i)
                UnitDPSDict.Add(SetUnits[i], new Queue<int>());
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
        UpgradeCostOfUnits[upgradeSlot] = Managers.UnitStatus.UnitUpgradLv[(UnitNames)SetUnits[upgradeSlot]] * ConstantData.BaseUpgradeCost;
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
            if (CurStageMonsterCount < stageData.monsterSpawnCount)
                SpawnMonster(_monsterSpawnPoint.transform.position);
        }
    }

    public void SpawnMonster(Vector3 spawnPoint)
    {
        GameObject go = Managers.Resource.Instantiate("Monster",newParentName:"Monsters");
        Monster monster = go.GetOrAddComponent<Monster>();
        monster.Init(CurStage, CurMap);
        if (Managers.Data.StageDict[CurStage].isSpecial)
            go.transform.localScale = Vector3.one * 1.5f;
        else
            go.transform.localScale = Vector3.one;
        _monsters.Add(monster);
        CurStageMonsterCount++;

        if (Monsters.Count > ConstantData.MonsterCountForGameOver)
        {
            GameOver("Fail");
            return;
        }
        go.transform.position = spawnPoint;
    }

    public void GameOver(string gameoverType)
    {
        SetPlayerHighestStage();
        Managers.Time.GamePause();
        Managers.UI.ShowPopupUI<UI_GameOver>().SetUp(gameoverType);
        Managers.Player.Data.amountOfGold += EarnedGoldCoin;
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
            if (Util.CheckTheEventAndCall(OnSellAUnit, SelectedUnit.GetComponent<Unit>(), sellCost))
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
        if (SelectedUnit != null && SelectUnitInfoUI != null)
        {
            Managers.UI.CloseWorldSpaceUI(SelectUnitInfoUI.gameObject);
            SelectedUnit = null;
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

    public void AddDamagesInDPSQueue(UnitNames unit,int damage)
    {
        if(UnitDPSDict.TryGetValue(unit,out Queue<int> dpsQ))
        {
            dpsQ.Enqueue(damage);
            StartCoroutine(CoRemoveDPSInOverSecond(unit));
        }
    }

    IEnumerator CoRemoveDPSInOverSecond(UnitNames unit)
    {
        yield return _aSecond;
        UnitDPSDict[unit].TryDequeue(out int t);
        Util.CheckTheEventAndCall(OnDPSChecker);
    }

    public void Clear()
    {
        ClearBindedEvent();
    }
}
