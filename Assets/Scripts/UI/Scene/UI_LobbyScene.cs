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
        RuneSlotsScroll,
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
        #endregion
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
        #endregion
    }

    public override void Init()
    {
        SetCanvasForLobbyScene();
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        Get<GameObject>((int)GameObjects.NestedScrollManager).GetOrAddComponent<NestedScrollManager>().Init();
        Get<GameObject>((int)GameObjects.UnitSlotsScroll).GetOrAddComponent<ScrollScript>()
            .Init("UnitSlots", "UnitSlotsViewport", "UnitSlotsScrollBar");
        Get<GameObject>((int)GameObjects.RuneSlotsScroll).GetOrAddComponent<ScrollScript>()
            .Init("RuneSlots", "RuneSlotsViewport", "RuneSlotsScrollBar");

        GetTMPro((int)Texts.TextTabCombat).text = Language.Combat;
        GetTMPro((int)Texts.TextTabBarrack).text = Language.Barrack;
        GetTMPro((int)Texts.TextTabRune).text = Language.Rune;

        #region CombatPanel

        GetTMPro((int)Texts.TextStartCombat).text = Language.StartCombat;

        GetButton((int)Buttons.BtnStartCombat).gameObject.AddUIEvent(OnStartCombatButtonClicked);
        GetButton((int)Buttons.BtnOptionMenu).gameObject.AddUIEvent(OnOptionMenuButtonClicked);

        #endregion

        #region BarrackPanel
        GetTMPro((int)Texts.BtnUseUnitText).text = Language.Use;
        GetTMPro((int)Texts.Lv3PreviewText).text = Language.Lv3Preview;

        // 현재 사용가능한 유닛들을 슬롯에 세팅
        _unitSlotsTF = Get<GameObject>((int)GameObjects.UnitSlots).transform;
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

        SelectUnitPanel = Get<GameObject>((int)GameObjects.SelectUnitPanel).GetComponent<SelectedPanel>();

        GetButton((int)Buttons.BtnUseUnit).gameObject.AddUIEvent(OnUseUnitButtonClick);
        GetButton((int)Buttons.BtnCloseSelectedUnit).gameObject.AddUIEvent(OnCloseSelectedUnitPanelButton);

        #endregion

        #region RunePanel
        _runeSlotsTF = Get<GameObject>((int)GameObjects.RuneSlots).transform;

        SetOwnRunes();

        Transform equipedRunesTF = Get<GameObject>((int)GameObjects.SetRunesPanel).transform;
        for (int index = 0; index < ConstantData.EquipedRunesCount; index++)
        {
            GameObject equipedRuneSlot = Managers.UI.MakeSubItem<UI_EquipedRuneSlot>(parent : equipedRunesTF).gameObject;
            equipedRuneSlot.transform.localScale = new Vector3(1f, 1f, 1f);
            _equipedRunes[index] = equipedRuneSlot;
        }

        SetEquipedRunes();

        SelectRunePanel = Get<GameObject>((int)GameObjects.SelectRunePanel).GetComponent<SelectedPanel>();

        GetButton((int)Buttons.BtnCloseSelectedRune).gameObject.AddUIEvent(OnCloseSelectedRunePanelButton);
        GetButton((int)Buttons.BtnUseRune).gameObject.AddUIEvent(OnUseRuneButtonClick);
        GetButton((int)Buttons.BtnSellRune).gameObject.AddUIEvent(OnSellRuneButtonClick);
        GetButton((int)Buttons.BtnClearRune).gameObject.AddUIEvent(OnClearRuneButtonClick);

        GetButton((int)Buttons.OneRuneGambleButton).gameObject.AddUIEvent(OnOneRuneGambleButtonClicked);
        GetButton((int)Buttons.TenRuneGambleButton).gameObject.AddUIEvent(OnTenRuneGambleButtonClicked);
        #endregion

        // 설정된 유닛 슬롯 초기화
        Transform setUnitsTF = Get<GameObject>((int)GameObjects.SetUnitPanel).transform;
        for (int index = 0; index < ConstantData.SetUnitCount; index++)
        {
            GameObject setUnit = Managers.UI.MakeSubItem<UI_SetUnit>(parent : setUnitsTF).gameObject;
            setUnit.transform.localScale = new Vector3(1f, 1f, 1f);
            _barrackSetUnits[index] = setUnit;

            GameObject setUnit2 = Managers.UI.MakeSubItem<UI_SetUnit>(parent : Get<GameObject>(index).transform).gameObject;
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

    private void SetCanvasForLobbyScene()
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(this.gameObject);
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;

        canvas.overrideSorting = true;

        canvas.sortingOrder = ConstantData.SceneUISortOrder;
    }

    public override void OnChangeLanguage()
    {
        base.OnChangeLanguage();
        GetTMPro((int)Texts.TextTabCombat).text = Language.Combat;
        GetTMPro((int)Texts.TextTabBarrack).text = Language.Barrack;
        GetTMPro((int)Texts.TextTabRune).text = Language.Rune;
        SetStartCombatButton(_setAllUnits);

        if (_selectedUnitId != UnitNames.Count)
            UpdateSelectedUnitPanel(_selectedUnitId);

        if (selectedRune != null)
            UpdateSelectedRunePanel(selectedRune, selectedRuneIndex);
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
        Get<GameObject>((int)GameObjects.BlockerStartCombatBtn).SetActive(!setAllUnits);
        GetButton((int)Buttons.BtnStartCombat).interactable = setAllUnits;
        string text = setAllUnits ? Language.StartCombat : Language.CompleteSetUnits;
        GetTMPro((int)Texts.TextStartCombat).text = text;

    }

    #endregion

    #region BarrackPanel

    // 유닛슬롯에서 유닛 선택 시 선택된 유닛 정보 및 사용하기 버튼이 있는 선택유닛 패널을 표시함
    UnitNames _selectedUnitId = UnitNames.Count;
    public void UpdateSelectedUnitPanel(UnitNames unitId)
    {
        _selectedUnitId = unitId;
        SelectUnitPanel.ShowSelectedPanel();

        GetTMPro((int)Texts.UnitNameText).text = Language.GetBaseUnitName(unitId);

        // UnitPrefabPoint(선택된 유닛의 프리펩이 담겨있는 위치)에 기존에 선택된 유닛이 있을 때 제거함
        if (Util.FindChild(Get<GameObject>((int)GameObjects.UnitPrefabPoint)) != null)
        {
            Managers.Resource.Destroy(Util.FindChild(Get<GameObject>((int)GameObjects.UnitPrefabPoint)));
        }

        GameObject obj = Managers.Resource.Instantiate($"Units/{unitId}_3", Get<GameObject>((int)GameObjects.UnitPrefabPoint).transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.identity;

        Util.FindChild<SortingGroup>(obj).sortingOrder = ConstantData.SelectedUnitPrefabSortOrder;

        Animator selectedUnitPrefabAnimator = Util.FindChild<Animator>(obj);
        selectedUnitPrefabAnimator.runtimeAnimatorController 
            = Managers.Resource.Load<RuntimeAnimatorController>("Animations/PrefabUnits/AnimatorController");
        selectedUnitPrefabAnimator.SetFloat("AttackAnimSpeed", 1f);
        selectedUnitPrefabAnimator.Play($"Attack_{Managers.Data.BaseUnitDict[(int)unitId].job}",0);

        string infoName,InfoValue;
        SetUnitInfoText(unitId, out infoName, out InfoValue);
        GetTMPro((int)Texts.UnitInfoText).text = infoName;
        GetTMPro((int)Texts.UnitInfoText2).text = InfoValue;
        GetTMPro((int)Texts.Lv3PreviewText).text = Language.Lv3Preview;
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
    public void SetOwnRunes()
    {
        for (int i = 0; i < _runeSlotsTF.childCount; ++i)
        {
            Managers.Resource.Destroy(_runeSlotsTF.GetChild(i).gameObject);
        }

        for (int runeIndex = 0; runeIndex < Managers.Player.Data.ownedRunes.Count; runeIndex++)
        {
            GameObject ownRune = Managers.UI.MakeSubItem<UI_RuneSlot>(parent : _runeSlotsTF).gameObject;
            ownRune.transform.localScale = new Vector3(1f, 1f, 1f);

            ownRune.GetComponent<UI_RuneSlot>().SetRune(runeIndex, this);
        }
        Managers.Player.SaveToJson();
    }

    public void SetEquipedRunes()
    {
        Rune[] equipedRunes = Managers.Player.Data.EquipedRunes;
        for (int i = 0; i < ConstantData.EquipedRunesCount; i++)
        {
            if (equipedRunes[i] != null && equipedRunes[i].equipSlotIndex != -1)
            {
                Rune rune = equipedRunes[i];
                _equipedRunes[i].GetComponent<UI_EquipedRuneSlot>().SetRune(rune,this);
            }
            else
                _equipedRunes[i].GetComponent<UI_EquipedRuneSlot>().OffImage();
        }
        // 장착 룬이 바뀌면 보유한 룬들도 한번 업데이트
        StartCoroutine("RuneSlotsUpdate");
        Managers.Player.SaveToJson();
    }

    Rune selectedRune = null;
    int selectedRuneIndex = -1;
    public void UpdateSelectedRunePanel(Rune rune, int runeIndex = -1)
    {
        selectedRune = rune;
        selectedRuneIndex = runeIndex;
        SelectRunePanel.ShowSelectedPanel();

        GetTMPro((int)Texts.RuneNameText).text = Language.GetRuneNameText(rune.baseRune);

        GetTMPro((int)Texts.RuneBaseInfoText).text = Language.GetRuneBaseInfo(rune.baseRune, rune.baseRuneEffectValue);

        GetTMPro((int)Texts.RuneSubInfoText).text = "";
        int additionalEffectCount = rune.additionalEffects.Count;
        for (int i = 0; i < additionalEffectCount; ++i)
        {
            AdditionalEffectOfRune effect = rune.additionalEffects[i];
            string additionalEffectText = Language.GetRuneAdditionalEffectText(effect.name,effect.value);
            GetTMPro((int)Texts.RuneSubInfoText).text += $"{additionalEffectText}\n";
        }

        // rundIndex 가 -1 이면 장착한 룬을 선택한 경우
        bool clickEquipedRune = runeIndex == -1;
        Get<GameObject>((int)GameObjects.BtnClearRune).gameObject.SetActive(clickEquipedRune);
        Get<GameObject>((int)GameObjects.BtnUseRune).gameObject.SetActive(!clickEquipedRune);
        Get<GameObject>((int)GameObjects.BtnSellRune).gameObject.SetActive(!clickEquipedRune);
        if (clickEquipedRune)
        {
            GetTMPro((int)Texts.BtnClearRuneText).text = Language.Clear;
        }
        else
        {
            GetTMPro((int)Texts.BtnUseRuneText).text = Language.Use;
            // TEMP
            int sellCost = 100;
            GetTMPro((int)Texts.BtnSellRuneText).text = $"{Language.Sell} {sellCost}";
        }

        GetTMPro((int)Texts.RuneGradeText).text = Language.GetRuneGradeText(rune.gradeOfRune);
        GetTMPro((int)Texts.RuneGradeText).color = ConstantData.RuneGradeColors[(int)rune.gradeOfRune];

        GameObject runeImageObj = Get<GameObject>((int)GameObjects.RuneImageBack);
        Util.FindChild<Image>(runeImageObj, "RuneImage").sprite = Managers.Rune.RuneSprites[rune.gradeOfRune];
        Util.FindChild<TextMeshProUGUI>(runeImageObj, "RuneImageText", true).text = Managers.Rune.RuneTextImages[rune.baseRune];
    }

    public void OnUseRuneButtonClick(PointerEventData eventData)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        Managers.UI.ShowPopupUI<UI_UseRune>().Set(selectedRune);
    }

    public void OnClearRuneButtonClick(PointerEventData eventData)
    {
        SelectRunePanel.HideSelectedPanel();

        Managers.Sound.Play(Define.SFXNames.Click);
        PlayerData playerData = Managers.Player.Data;
        for(int i = 0; i < playerData.EquipedRunes.Length; ++i)
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

    public void OnSellRuneButtonClick(PointerEventData eventData)
    {
        SelectRunePanel.HideSelectedPanel();

        Managers.Sound.Play(Define.SFXNames.Click);
        if (selectedRuneIndex == -1 || Managers.Player.Data.ownedRunes[selectedRuneIndex].isEquip == true)
            return;
        Managers.Resource.Destroy(_runeSlotsTF.GetChild(selectedRuneIndex).gameObject);
        Managers.Player.Data.ownedRunes.RemoveAt(selectedRuneIndex);

        StartCoroutine("RuneSlotsUpdate");
        Managers.Player.SaveToJson();
    }

    IEnumerator RuneSlotsUpdate()
    {
        yield return null;
        for (int runeIndex = 0; runeIndex < Managers.Player.Data.ownedRunes.Count; runeIndex++)
        {
            GameObject ownRune = _runeSlotsTF.GetChild(runeIndex).gameObject;

            ownRune.GetComponent<UI_RuneSlot>().SetRune(runeIndex, this);
        }
    }

    public void OnCloseSelectedRunePanelButton(PointerEventData eventData)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        SelectRunePanel.HideSelectedPanel();
    }

    public void OnOneRuneGambleButtonClicked(PointerEventData eventData)
    {
        // 플레이어 골드 확인
        Managers.Player.Data.ownedRunes.Add(Managers.Rune.CreateRandomRune());

        GameObject newRune = Managers.UI.MakeSubItem<UI_RuneSlot>(parent : _runeSlotsTF).gameObject;
        newRune.transform.localScale = new Vector3(1f, 1f, 1f);

        newRune.GetComponent<UI_RuneSlot>().SetRune(Managers.Player.Data.ownedRunes.Count - 1, this);
        Managers.Player.SaveToJson();
    }

    public void OnTenRuneGambleButtonClicked(PointerEventData eventData)
    {
        // 플레이어 골드 확인

        int beginRuneIndex = Managers.Player.Data.ownedRunes.Count;
        for (int count = 0; count < 10; count++)
        {
            Managers.Player.Data.ownedRunes.Add(Managers.Rune.CreateRandomRune());
        }
        Managers.Player.Data.ownedRunes.Add(Managers.Rune.CreateRandomRune(GradeOfRune.Unique));

        for (int runeIndex = beginRuneIndex; runeIndex < Managers.Player.Data.ownedRunes.Count; runeIndex++)
        {
            GameObject newRune = Managers.UI.MakeSubItem<UI_RuneSlot>(parent : _runeSlotsTF).gameObject;
            newRune.transform.localScale = new Vector3(1f, 1f, 1f);

            newRune.GetComponent<UI_RuneSlot>().SetRune(runeIndex, this);
        }
        Managers.Player.SaveToJson();
    }
    #endregion
    public void Clear()
    {
        Managers.Resource.Destroy(gameObject);
    }
}
