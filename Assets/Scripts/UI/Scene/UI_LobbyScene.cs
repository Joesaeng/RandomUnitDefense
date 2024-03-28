using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    public LobbyScene Scene { get; set; }

    SelectedPanel SelectUnitPanel { get; set; }
    SelectedPanel SelectRunePanel { get; set; }
    Transform _unitSlotsTF;
    Transform _runeSlotsTF;

    GameObject[] _barrackSetUnits = new GameObject[ConstantData.SetUnitCount];
    GameObject[] _combatSetUnits = new GameObject[ConstantData.SetUnitCount];

    GameObject[] _equipedRunes = new GameObject[ConstantData.EquipedRunesCount];

    enum GameObjects
    {
        #region CombatPanel
        Point0,
        Point1,
        Point2,
        Point3,
        Point4,
        BlockerStartCombatBtn,
        #endregion
        #region BarrackPanel
        UnitSlots,
        UnitSlotsScroll,
        SelectUnitPanel,
        UnitPrefabPoint,
        SetUnitPanel,
        #endregion
        #region RunePanel
        RuneSlots,
        // RuneSlotsScroll,
        SelectRunePanel,
        RuneImageBack,
        SetRunesPanel,
        BtnUseRune,
        BtnClearRune,
        BtnSellRune,
        #endregion
        NestedScrollManager,
    }

    enum Texts
    {
        TextTabCombat,
        TextTabBarrack,
        TextTabRune,
        #region CombatPanel
        TextStartCombat,
        #endregion
        #region BarrackPanel
        UnitNameText,
        UnitInfoText,
        UnitInfoText2,
        BtnUseUnitText,
        Lv3PreviewText,
        #endregion
        #region RunePanel
        RuneNameText,
        RuneBaseInfoText,
        RuneSubInfoText,
        BtnUseRuneText,
        BtnClearRuneText,
        BtnSellRuneText,
        RuneGradeText,
        OneRuneGambleText,
        TenRuneGambleText,
        GambleOneRuneCostText,
        GambleTenRuneCostText,
        SortRunesText,
        SellAllRunesText,
        CurListText,
        LastListText,
        #endregion
        TextAmountGold,
    }

    enum Buttons
    {
        BtnOptionMenu,
        #region CombatPanel
        BtnStartCombat,
        #endregion
        #region BarrackPanel
        BtnCloseSelectedUnit,
        BtnUseUnit,
        #endregion
        #region RunePanel
        BtnCloseSelectedRune,
        BtnUseRune,
        BtnClearRune,
        BtnSellRune,
        OneRuneGambleButton,
        TenRuneGambleButton,
        BtnSortRunes,
        BtnSellAllRunes,
        ListPrevButton,
        ListNextButton,
        #endregion
    }

    public override void Init()
    {
        base.Init();
        SetCanvasRenderModeCamera();
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetObject((int)GameObjects.NestedScrollManager).GetOrAddComponent<NestedScrollManager>().Init();
        GetObject((int)GameObjects.UnitSlotsScroll).GetOrAddComponent<ScrollScript>()
            .Init("UnitSlots", "UnitSlotsViewport", "UnitSlotsScrollBar");

        OnChangeLanguage();



        GetText((int)Texts.TextAmountGold).text = $"{Managers.Player.Data.amountOfGold}";

        #region CombatPanel

        GetText((int)Texts.TextStartCombat).text = Language.StartCombat;

        GetButton((int)Buttons.BtnStartCombat).gameObject.AddUIEvent(OnStartCombatButtonClicked);
        GetButton((int)Buttons.BtnOptionMenu).gameObject.AddUIEvent(OnOptionMenuButtonClicked);

        #endregion

        #region BarrackPanel
        GetText((int)Texts.BtnUseUnitText).text = Language.Use;
        GetText((int)Texts.Lv3PreviewText).text = Language.Lv3Preview;

        // 현재 사용가능한 유닛들을 슬롯에 세팅
        _unitSlotsTF = GetObject((int)GameObjects.UnitSlots).transform;
        int baseSlotCount = 30;
        for (int i = 0; i < baseSlotCount; i++)
        {
            GameObject unitSlot = Managers.UI.MakeSubItem<UI_UnitSlot>(parent : _unitSlotsTF).gameObject;
            unitSlot.transform.localScale = new Vector3(1f, 1f, 1f);
            int unitId = i + ConstantData.FirstOfUnitID;
            if (unitId < (int)UnitNames.Count)
            {
                unitSlot.GetComponent<UI_UnitSlot>().Set((UnitNames)unitId, this);
            }
        }

        SelectUnitPanel = GetObject((int)GameObjects.SelectUnitPanel).GetComponent<SelectedPanel>();
        SelectUnitPanel.gameObject.SetActive(false);
        GetButton((int)Buttons.BtnUseUnit).gameObject.AddUIEvent(OnUseUnitButtonClick);
        GetButton((int)Buttons.BtnCloseSelectedUnit).gameObject.AddUIEvent(OnCloseSelectedUnitPanelButton);

        #endregion

        #region RunePanel
        _runeSlotsTF = GetObject((int)GameObjects.RuneSlots).transform;

        // 룬슬롯 리스트 개수 초기화
        int runeCount = Managers.Player.Data.ownedRunes.Count;

        _curListNum = 0;
        _lastListNum = (int)Mathf.Floor(runeCount / RuneCountOfTheOneList);

        GetText((int)Texts.CurListText).text = $"{_curListNum + 1}";
        GetText((int)Texts.LastListText).text = $"{_lastListNum + 1}";

        GetText((int)Texts.GambleOneRuneCostText).text = $"{ConstantData.TheCostOfOneRuneGamble}";
        GetText((int)Texts.GambleTenRuneCostText).text = $"{ConstantData.TheCostOfTenRunesGamble}";
        //

        Managers.Rune.SortOwnedRunes((Define.SortModeOfRunes)Managers.Player.Data.sortModeOfRune);

        StartCoroutine(CoSetOwnRunes());
        Transform equipedRunesTF = GetObject((int)GameObjects.SetRunesPanel).transform;
        for (int index = 0; index < ConstantData.EquipedRunesCount; index++)
        {
            GameObject equipedRuneSlot = Managers.UI.MakeSubItem<UI_EquipedRuneSlot>(parent : equipedRunesTF).gameObject;
            equipedRuneSlot.transform.localScale = new Vector3(1f, 1f, 1f);
            _equipedRunes[index] = equipedRuneSlot;
        }

        SetEquipedRunes();

        SelectRunePanel = GetObject((int)GameObjects.SelectRunePanel).GetComponent<SelectedPanel>();
        SelectRunePanel.gameObject.SetActive(false);

        GetButton((int)Buttons.BtnCloseSelectedRune).gameObject.AddUIEvent(OnCloseSelectedRunePanelButton);
        GetButton((int)Buttons.BtnUseRune).gameObject.AddUIEvent(OnUseRuneButtonClick);
        GetButton((int)Buttons.BtnSellRune).gameObject.AddUIEvent(OnSellRuneButtonClick);
        GetButton((int)Buttons.BtnClearRune).gameObject.AddUIEvent(OnClearRuneButtonClick);

        GetButton((int)Buttons.OneRuneGambleButton).gameObject.AddUIEvent(OnOneRuneGambleButtonClicked);
        GetButton((int)Buttons.TenRuneGambleButton).gameObject.AddUIEvent(OnTenRuneGambleButtonClicked);

        GetButton((int)Buttons.BtnSortRunes).gameObject.AddUIEvent(OnSortRunesButtonClicked);
        GetButton((int)Buttons.BtnSellAllRunes).gameObject.AddUIEvent(OnSellAllRunesButtonClicked);

        GetButton((int)Buttons.ListPrevButton).gameObject.AddUIEvent(OnListPrevButtonClicked);
        GetButton((int)Buttons.ListNextButton).gameObject.AddUIEvent(OnListNextButtonClicked);
        #endregion

        // 설정된 유닛 슬롯 초기화
        Transform setUnitsTF = GetObject((int)GameObjects.SetUnitPanel).transform;
        for (int index = 0; index < ConstantData.SetUnitCount; index++)
        {
            GameObject setUnit = Managers.UI.MakeSubItem<UI_SetUnit>(parent : setUnitsTF).gameObject;
            setUnit.transform.localScale = new Vector3(1f, 1f, 1f);
            _barrackSetUnits[index] = setUnit;

            GameObject setUnit2 = Managers.UI.MakeSubItem<UI_SetUnit>(parent : GetObject(index).transform).gameObject;
            setUnit2.transform.localScale = new Vector3(1f, 1f, 1f);
            setUnit2.transform.localPosition = Vector3.zero;
            setUnit2.GetComponent<Image>().enabled = false;
            setUnit2.GetComponent<UI_SetUnit>().SelectImageOff();
            _combatSetUnits[index] = setUnit2;
        }

        SetSetUnits();


    }

    // 사용할 유닛들 세팅
    bool _setAllUnits;
    public void SetSetUnits()
    {
        _setAllUnits = true;

        for (int index = 0; index < ConstantData.SetUnitCount; ++index)
        {
            if (Managers.Player.Data.setUnits[index] != 0)
            {
                _barrackSetUnits[index].GetComponent<UI_SetUnit>().SetImage($"Art/Units/{(UnitNames)Managers.Player.Data.setUnits[index]}");
                _combatSetUnits[index].GetComponent<UI_SetUnit>().SetImage($"Art/Units/{(UnitNames)Managers.Player.Data.setUnits[index]}");
            }
            else
            {
                _setAllUnits = false;
                _barrackSetUnits[index].GetComponent<UI_SetUnit>().SetImage();
                _combatSetUnits[index].GetComponent<UI_SetUnit>().OffImage();
            }
        }

        // 모든 유닛이 설정되지 않았다면 전투 시작 버튼을 비활성화함
        SetStartCombatButton(_setAllUnits);

        Managers.Player.SaveToJson();
    }

    public override void OnChangeLanguage()
    {
        base.OnChangeLanguage();

        GetText((int)Texts.TextTabCombat).text = Language.Combat;
        GetText((int)Texts.TextTabBarrack).text = Language.Barrack;
        GetText((int)Texts.TextTabRune).text = Language.Rune;

        GetText((int)Texts.OneRuneGambleText).text = Language.OneRuneGambleText;
        GetText((int)Texts.TenRuneGambleText).text = Language.TenRuneGamblesText;
        GetText((int)Texts.SortRunesText).text =
            Language.GetRuneSortMode((Define.SortModeOfRunes)Managers.Player.Data.sortModeOfRune);
        GetText((int)Texts.SellAllRunesText).text = Language.SellAll;

        SetStartCombatButton(_setAllUnits);

        if (_selectedUnitId != UnitNames.Count)
            UpdateSelectedUnitPanel(_selectedUnitId);

        if (selectedRune != null)
            UpdateSelectedRunePanel(selectedRune, selectedRuneIndex);
    }

    private void UpdateAmountOfGoldText()
    {
        GetText((int)Texts.TextAmountGold).text = $"{Managers.Player.Data.amountOfGold}";
    }

    public void OnOptionMenuButtonClicked(PointerEventData data)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        Managers.UI.ShowPopupUI<UI_OptionMenu>();
    }

    #region CombatPanel

    public void OnStartCombatButtonClicked(PointerEventData data)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        Managers.Scene.LoadSceneWithLoadingScene(Define.Scene.Combat);
    }

    private void SetStartCombatButton(bool setAllUnits)
    {
        GetObject((int)GameObjects.BlockerStartCombatBtn).SetActive(!setAllUnits);
        GetButton((int)Buttons.BtnStartCombat).interactable = setAllUnits;
        string text = setAllUnits ? Language.StartCombat : Language.CompleteSetUnits;
        GetText((int)Texts.TextStartCombat).text = text;
    }

    #endregion

    #region BarrackPanel

    // 유닛슬롯에서 유닛 선택 시 선택된 유닛 정보 및 사용하기 버튼이 있는 선택유닛 패널을 표시함
    UnitNames _selectedUnitId = UnitNames.Count;
    public void UpdateSelectedUnitPanel(UnitNames unitId)
    {
        _selectedUnitId = unitId;
        SelectUnitPanel.gameObject.SetActive(true);
        SelectUnitPanel.ShowSelectedPanel();

        GetText((int)Texts.UnitNameText).text = Language.GetBaseUnitName(unitId);

        // UnitPrefabPoint(선택된 유닛의 프리펩이 담겨있는 위치)에 기존에 선택된 유닛이 있을 때 제거함
        if (Util.FindChild(GetObject((int)GameObjects.UnitPrefabPoint)) != null)
        {
            Managers.Resource.Destroy(Util.FindChild(GetObject((int)GameObjects.UnitPrefabPoint)));
        }

        GameObject obj = Managers.Resource.Instantiate($"Units/{unitId}_3", GetObject((int)GameObjects.UnitPrefabPoint).transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.identity;

        Util.FindChild<SortingGroup>(obj).sortingOrder = ConstantData.SelectedUnitPrefabSortOrder;

        Animator selectedUnitPrefabAnimator = Util.FindChild<Animator>(obj);
        selectedUnitPrefabAnimator.runtimeAnimatorController
            = Managers.Resource.Load<RuntimeAnimatorController>("Animations/PrefabUnits/AnimatorController");
        selectedUnitPrefabAnimator.SetFloat("AttackAnimSpeed", 1f);
        selectedUnitPrefabAnimator.Play($"Attack_{Managers.Data.BaseUnitDict[(int)unitId].job}", 0);

        string infoName,InfoValue;
        SetUnitInfoText(unitId, out infoName, out InfoValue);
        GetText((int)Texts.UnitInfoText).text = infoName;
        GetText((int)Texts.UnitInfoText2).text = InfoValue;
        GetText((int)Texts.Lv3PreviewText).text = Language.Lv3Preview;
    }

    public void OnUseUnitButtonClick(PointerEventData eventData)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        Managers.UI.ShowPopupUI<UI_UseUnit>().Set((int)_selectedUnitId);
    }

    // 유닛의 정보들을 세팅
    private void SetUnitInfoText(UnitNames unitId, out string infoName, out string infoValue)
    {
        string info =
            $"{Language.GetUnitInfo(Language.UnitInfos.AttackType)}\n" +
            $"{Language.GetUnitInfo(Language.UnitInfos.AttackDamage)}\n" +
            $"{Language.GetUnitInfo(Language.UnitInfos.AttackRange)}\n" +
            $"{Language.GetUnitInfo(Language.UnitInfos.AttackRate)}\n";
        string info2 = "";
        UnitStat_Base unitStat = Managers.Data.GetUnitData(unitId,1);

        switch (unitId)
        {
            case UnitNames.Knight:
            case UnitNames.Spearman:
            case UnitNames.Archer:
            {
                info2 += $"{Language.GetAttackType(UnitType.Common)}\n" +
                    $"{unitStat.attackDamage}\n" +
                    $"{unitStat.attackRange}\n" +
                    $"{Language.AttackPerNSeconds(unitStat.attackRate)}";
                break;
            }
            case UnitNames.FireMagician:
            case UnitNames.Viking:
            case UnitNames.Warrior:
            {
                info += $"{Language.GetUnitInfo(Language.UnitInfos.WideAttackArea)}";

                AOE aoe = unitStat as AOE;

                info2 += $"{Language.GetAttackType(UnitType.AOE)}\n" +
                    $"{unitStat.attackDamage}\n" +
                    $"{unitStat.attackRange}\n" +
                    $"{Language.AttackPerNSeconds(unitStat.attackRate)}\n" +
                    $"{aoe.wideAttackArea}";
                break;
            }
            case UnitNames.SlowMagician:
            {
                info += $"{Language.GetUnitInfo(Language.UnitInfos.WideAttackArea)}\n" +
                    $"{Language.GetUnitInfo(Language.UnitInfos.SlowRatio)}\n" +
                    $"{Language.GetUnitInfo(Language.UnitInfos.SlowDuration)}";

                SlowMagician slowMagician = unitStat as SlowMagician;

                info2 += $"{Language.GetAttackType(UnitType.Debuffer)}\n" +
                    $"{unitStat.attackDamage}\n" +
                    $"{unitStat.attackRange}\n" +
                    $"{Language.AttackPerNSeconds(unitStat.attackRate)}\n" +
                    $"{slowMagician.wideAttackArea}\n" +
                    $"{slowMagician.slowRatio * 100}%\n" +
                    $"{slowMagician.slowDuration}s";
                break;
            }
            case UnitNames.StunGun:
            {
                info += $"{Language.GetUnitInfo(Language.UnitInfos.StunDuration)}";

                StunGun stungun = unitStat as StunGun;

                info2 += $"{Language.GetAttackType(UnitType.Debuffer)}\n" +
                    $"{unitStat.attackDamage}\n" +
                    $"{unitStat.attackRange}\n" +
                    $"{Language.AttackPerNSeconds(unitStat.attackRate)}\n" +
                    $"{stungun.stunDuration}s";
                break;
            }
            case UnitNames.PoisonBowMan:
            {
                info += $"{Language.GetUnitInfo(Language.UnitInfos.PosionDamagePerSecond)}\n" +
                    $"{Language.GetUnitInfo(Language.UnitInfos.PosionDuration)}";

                PoisonBowMan poisonBowMan = unitStat as PoisonBowMan;

                info2 += $"{Language.GetAttackType(UnitType.Debuffer)}\n" +
                    $"{unitStat.attackDamage}\n" +
                    $"{unitStat.attackRange}\n" +
                    $"{Language.AttackPerNSeconds(unitStat.attackRate)}\n" +
                    $"{poisonBowMan.poisonDamagePerSecond}/s\n" +
                    $"{poisonBowMan.poisonDuration}s";
                break;
            }
        }

        infoName = info;
        infoValue = info2;
    }

    public void OnCloseSelectedUnitPanelButton(PointerEventData eventData)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        SelectUnitPanel.HideSelectedPanel();
    }

    #endregion

    #region RunePanel
    // 플레이어가 가지고있는 룬들을 UI에 표시
    int _curListNum = 0;
    int _lastListNum = 0;
    const int RuneCountOfTheOneList = 15;
    IEnumerator CoSetOwnRunes()
    {
        yield return null;
        int runeCount = Managers.Player.Data.ownedRunes.Count;

        // 1을 빼주는 이유는 룬의 개수가 RuneCountOfTheOneList의 배수일 때 비어있는 리스트가 생기기 때문
        _lastListNum = (int)Mathf.Floor((runeCount - 1) / RuneCountOfTheOneList);

        if (_curListNum > _lastListNum)
            _curListNum = _lastListNum;

        GetText((int)Texts.CurListText).text = $"{_curListNum + 1}";
        GetText((int)Texts.LastListText).text = $"{_lastListNum + 1}";

        int removeCount = 0;
        // 현재 리스트가 마지막 리스트 일 때
        if (_curListNum == _lastListNum)
        {
            // 삭제해야 할 슬롯의 개수를 찾음
            removeCount = (RuneCountOfTheOneList * (_curListNum + 1)) - runeCount;
        }

        // 현재 슬롯이 채워진 개수가 현재 페이지에 표시될 룬의 개수보다 작다면
        while (_runeSlotsTF.childCount < RuneCountOfTheOneList - removeCount)
        {
            // 슬롯을 만들어 채운다
            Transform ownRune = Managers.UI.MakeSubItem<UI_RuneSlot>(parent: _runeSlotsTF).transform;
            ownRune.localScale = new Vector3(1f, 1f, 1f);
        }

        // 현재 리스트의 첫번째 룬 인덱스를 구한다. ex) 0, 15, 30 . . .
        int curListFirstRuneIndex = _curListNum * RuneCountOfTheOneList;
        // 현재 리스트의 마지막 룬 인덱스를 구한다. ex) 14, 29, 44 . . .
        int curListLastRuneIndex = curListFirstRuneIndex + RuneCountOfTheOneList - 1;
        for (int runeIndex = curListLastRuneIndex;
            runeIndex >= curListFirstRuneIndex; --runeIndex)
        {
            // 역순으로 돌면서 룬 슬롯 오브젝트가 있는지, 있으면 있어야하는지 확인 
            if (_runeSlotsTF.TryGetChild(runeIndex - curListFirstRuneIndex, out Transform ownRune))
                if (runeIndex >= curListFirstRuneIndex + RuneCountOfTheOneList - removeCount)
                {
                    // 룬 슬롯 오브젝트는 존재하나 대응하는 룬이 없으면 제거
                    Managers.Resource.Destroy(ownRune.gameObject);
                    continue;
                }
            if (runeIndex < Managers.Player.Data.ownedRunes.Count)
                ownRune.GetComponent<UI_RuneSlot>().SetRune(runeIndex, this);
        }
    }

    // 룬 장착 혹은 해제 시 장착된 룬들을 설정
    public void SetEquipedRunes()
    {
        Rune[] equipedRunes = Managers.Player.Data.EquipedRunes;
        for (int i = 0; i < ConstantData.EquipedRunesCount; i++)
        {
            if (equipedRunes[i] != null && equipedRunes[i].equipSlotIndex != -1)
            {
                Rune rune = equipedRunes[i];
                _equipedRunes[i].GetComponent<UI_EquipedRuneSlot>().SetRune(rune, this);
            }
            else
                _equipedRunes[i].GetComponent<UI_EquipedRuneSlot>().OffImage();
        }
        // 장착 룬이 바뀌면 보유한 룬들도 한번 업데이트
        StartCoroutine(CoSetOwnRunes());
        Managers.Player.SaveToJson();
    }

    // 장착된 룬 혹은 보유중인 룬을 클릭했을 때 정보를 보여주는 패널 업데이트 및 보여줌
    Rune selectedRune = null;
    int selectedRuneIndex = -1;
    public void UpdateSelectedRunePanel(Rune rune, int runeIndex = -1)
    {
        selectedRune = rune;
        selectedRuneIndex = runeIndex;
        SelectRunePanel.gameObject.SetActive(true);
        SelectRunePanel.ShowSelectedPanel();

        GetText((int)Texts.RuneNameText).text = Language.GetRuneNameText(rune.baseRune);

        GetText((int)Texts.RuneBaseInfoText).text = Language.GetRuneBaseInfo(rune.baseRune, rune.baseRuneEffectValue);

        GetText((int)Texts.RuneSubInfoText).text = "";
        int additionalEffectCount = rune.additionalEffects.Count;
        for (int i = 0; i < additionalEffectCount; ++i)
        {
            AdditionalEffectOfRune effect = rune.additionalEffects[i];
            string additionalEffectText = Language.GetRuneAdditionalEffectText(effect.name,effect.value);
            GetText((int)Texts.RuneSubInfoText).text += $"{additionalEffectText}\n";
        }

        // rundIndex 가 -1 이면 장착한 룬을 선택한 경우
        bool clickEquipedRune = runeIndex == -1;
        GetObject((int)GameObjects.BtnClearRune).gameObject.SetActive(clickEquipedRune);
        GetObject((int)GameObjects.BtnUseRune).gameObject.SetActive(!clickEquipedRune);
        GetObject((int)GameObjects.BtnSellRune).gameObject.SetActive(!clickEquipedRune);

        if (clickEquipedRune)
        {
            GetText((int)Texts.BtnClearRuneText).text = Language.Clear;
        }
        else
        {
            GetText((int)Texts.BtnUseRuneText).text = Language.Use;
            // TEMP
            int sellCost = ConstantData.RuneSellingPrices[(int)rune.gradeOfRune];
            GetText((int)Texts.BtnSellRuneText).text = $"{Language.Sell} {sellCost}";
        }

        GetText((int)Texts.RuneGradeText).text = Language.GetRuneGradeText(rune.gradeOfRune);
        GetText((int)Texts.RuneGradeText).color = ConstantData.RuneGradeColors[(int)rune.gradeOfRune];

        GameObject runeImageObj = GetObject((int)GameObjects.RuneImageBack);
        Util.FindChild<Image>(runeImageObj, "Image").sprite = Managers.Rune.RuneSprites[rune.gradeOfRune];
        Util.FindChild<Image>(runeImageObj, "RuneTextImage", true).sprite = Managers.Rune.RuneTextImages[rune.baseRune];
    }

    // 룬 정보 패널에서 사용하기 버튼을 클릭했을 때 호출됨
    public void OnUseRuneButtonClick(PointerEventData eventData)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        Managers.UI.ShowPopupUI<UI_UseRune>().Set(selectedRune);
    }

    public void OnListPrevButtonClicked(PointerEventData eventData)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        if (_curListNum > 0)
            _curListNum--;
        else
            _curListNum = _lastListNum;
        StartCoroutine(CoSetOwnRunes());
    }

    public void OnListNextButtonClicked(PointerEventData eventData)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        if (_curListNum < _lastListNum)
            _curListNum++;
        else
            _curListNum = 0;
        StartCoroutine(CoSetOwnRunes());

    }

    // 룬 정보 패널에서 룬 해제(Clear)버튼 클릭했을 때 호출
    public void OnClearRuneButtonClick(PointerEventData eventData)
    {
        SelectRunePanel.HideSelectedPanel();

        Managers.Sound.Play(Define.SFXNames.Click);
        PlayerData playerData = Managers.Player.Data;
        for (int i = 0; i < playerData.EquipedRunes.Length; ++i)
        {
            if (playerData.EquipedRunes[i] == selectedRune)
            {
                playerData.EquipedRunes[i].isEquip = false;
                playerData.EquipedRunes[i].equipSlotIndex = -1;
                playerData.EquipedRunes[i] = null;
            }
        }
        SetEquipedRunes();
    }

    // 룬 정보 패널에서 룬 판매 버튼 클릭했을 때 호출
    public void OnSellRuneButtonClick(PointerEventData eventData)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        if (selectedRuneIndex == -1 || Managers.Player.Data.ownedRunes[selectedRuneIndex].isEquip == true)
        {
            Managers.UI.MakeSubItem<UI_NotificationText>().SetText(Define.NotiTexts.CannotSellEquipedRune);
            return;
        }

        SelectRunePanel.HideSelectedPanel();

        Managers.Player.Data.ownedRunes.RemoveAt(selectedRuneIndex);
        Managers.Player.Data.amountOfGold += ConstantData.RuneSellingPrices[(int)selectedRune.gradeOfRune];

        Managers.Player.SaveToJson();
        StartCoroutine(CoSetOwnRunes());
        UpdateAmountOfGoldText();
    }
    // 룬 일괄판매
    public void SellAllRunes(List<int> sellRunesIndexs, int sellPrices)
    {
        // sellRunesIndexs 는 정배열된 인덱스들을 받아오기 때문에 역순으로
        for (int i = sellRunesIndexs.Count - 1; i >= 0; --i)
        {
            Managers.Player.Data.ownedRunes.RemoveAt(sellRunesIndexs[i]);
        }

        Managers.Player.Data.amountOfGold += sellPrices;

        Managers.Player.SaveToJson();
        StartCoroutine(CoSetOwnRunes());
        UpdateAmountOfGoldText();
    }

    public void OnCloseSelectedRunePanelButton(PointerEventData eventData)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        SelectRunePanel.HideSelectedPanel();
    }

    public void OnOneRuneGambleButtonClicked(PointerEventData eventData)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        if (Managers.Player.Data.amountOfGold < ConstantData.TheCostOfOneRuneGamble)
        {
            Managers.UI.MakeSubItem<UI_NotificationText>().SetText(Define.NotiTexts.NotEnoughGoldCoin);
            return;
        }

        Managers.Player.Data.ownedRunes.Add(Managers.Rune.CreateRandomRune());

        Managers.Player.Data.amountOfGold -= ConstantData.TheCostOfOneRuneGamble;

        StartCoroutine(CoSetOwnRunes());
        Managers.Player.SaveToJson();
        UpdateAmountOfGoldText();
    }

    public void OnTenRuneGambleButtonClicked(PointerEventData eventData)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        if (Managers.Player.Data.amountOfGold < ConstantData.TheCostOfTenRunesGamble)
        {
            Managers.UI.MakeSubItem<UI_NotificationText>().SetText(Define.NotiTexts.NotEnoughGoldCoin);
            return;
        }

        for (int count = 0; count < 10; count++)
        {
            Managers.Player.Data.ownedRunes.Add(Managers.Rune.CreateRandomRune());
        }
        Managers.Player.Data.ownedRunes.Add(Managers.Rune.CreateRandomRune(GradeOfRune.Unique));

        Managers.Player.Data.amountOfGold -= ConstantData.TheCostOfTenRunesGamble;

        StartCoroutine(CoSetOwnRunes());
        Managers.Player.SaveToJson();
        UpdateAmountOfGoldText();
    }

    public void OnSortRunesButtonClicked(PointerEventData eventData)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        int sortMode = (Managers.Player.Data.sortModeOfRune + 1) % (int)Define.SortModeOfRunes.Count;
        Managers.Rune.SortOwnedRunes((Define.SortModeOfRunes)sortMode);
        Managers.Player.Data.sortModeOfRune = sortMode;
        GetText((int)Texts.SortRunesText).text = Language.GetRuneSortMode((Define.SortModeOfRunes)sortMode);
        StartCoroutine(CoSetOwnRunes());

        Managers.Player.SaveToJson();
    }
    public void OnSellAllRunesButtonClicked(PointerEventData eventData)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        // 룬 일괄판매 팝업
        Managers.UI.ShowPopupUI<UI_SellAllRunes>().Set(this);
    }
    #endregion
    public void Clear()
    {
        Managers.Resource.Destroy(gameObject);
    }
}
