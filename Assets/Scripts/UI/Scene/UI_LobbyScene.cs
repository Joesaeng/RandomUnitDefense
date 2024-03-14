using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    public LobbyScene Scene { get; set; }

    SelectUnitPanel SelectUnitPanel { get; set; }
    Transform _unitSlotsTF;

    GameObject[] _barrackSetUnits = new GameObject[ConstantData.SetUnitCount];
    GameObject[] _combatSetUnits = new GameObject[ConstantData.SetUnitCount];

    enum GameObjects
    {
        #region CombatPanel
        Point0,
        Point1,
        Point2,
        Point3,
        Point4,
        #endregion
        #region BarrackPanel
        UnitSlots,
        SelectUnitPanel,
        UnitPrefabPoint,
        SetUnitPanel,
        #endregion
        NestedScrollManager,
        UnitSlotsScroll,
    }

    enum Texts
    {
        TextTabCombat,
        TextTabBarrack,
        TextTabShop,
        #region CombatPanel
        TextStartCombat,
        #endregion
        #region BarrackPanel
        // SeletedUnitPanel
        UnitNameText,
        UnitInfoText,
        UnitInfoText2,
        BtnUseUnitText,
        Lv3PreviewText,
        #endregion
        #region ShopPanel
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
        #region ShopPanel
        #endregion
    }

    public override void Init()
    {
        SetCanvasForLobbyScene();
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        Get<GameObject>((int)GameObjects.NestedScrollManager).GetOrAddComponent<NestedScrollManager>().Init();
        Get<GameObject>((int)GameObjects.UnitSlotsScroll).GetOrAddComponent<ScrollScript>().Init();

        GetTMPro((int)Texts.TextTabCombat).text = Language.Combat;
        GetTMPro((int)Texts.TextTabBarrack).text = Language.Barrack;
        GetTMPro((int)Texts.TextTabShop).text = Language.Shop;

        #region CombatPanel
        GetTMPro((int)Texts.TextStartCombat).text = Language.StartCombat;

        GetButton((int)Buttons.BtnStartCombat).gameObject.AddUIEvent(OnStartCombatButtonClicked);
        GetButton((int)Buttons.BtnOptionMenu).gameObject.AddUIEvent(OnOptionMenuButtonClicked);
        #endregion

        #region BarrackPanel
        GetTMPro((int)Texts.BtnUseUnitText).text = Language.Use;
        GetTMPro((int)Texts.Lv3PreviewText).text = Language.Lv3Preview;

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
            _combatSetUnits[index] = setUnit2;
        }

        SetSetUnits();

        SelectUnitPanel = Get<GameObject>((int)GameObjects.SelectUnitPanel).GetComponent<SelectUnitPanel>();

        GetButton((int)Buttons.BtnUseUnit).gameObject.AddUIEvent(OnUseUnitButtonClick);
        GetButton((int)Buttons.BtnCloseSelectedUnit).gameObject.AddUIEvent(OnCloseSelectedUnitPanelButton);
        #endregion
    }
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

        SetStartCombatButton(_setAllUnits);
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
        GetTMPro((int)Texts.TextTabShop).text = Language.Shop;
        SetStartCombatButton(_setAllUnits);

        GetTMPro((int)Texts.BtnUseUnitText).text = Language.Use;
        GetTMPro((int)Texts.Lv3PreviewText).text = Language.Lv3Preview;
        GetTMPro((int)Texts.UnitNameText).text = Language.GetBaseUnitName(_selectedUnitId);

        string infoName,InfoValue;
        SetInfoText(_selectedUnitId, out infoName, out InfoValue);
        GetTMPro((int)Texts.UnitInfoText).text = infoName;
        GetTMPro((int)Texts.UnitInfoText2).text = InfoValue;
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
        Managers.Scene.LoadScene(Define.Scene.Combat);
    }
    
    private void SetStartCombatButton(bool setAllUnits)
    {
        GetButton((int)Buttons.BtnStartCombat).interactable = setAllUnits;
        string text = setAllUnits ? Language.StartCombat : Language.CompleteSetUnits;
        GetTMPro((int)Texts.TextStartCombat).text = text;

    }

    #endregion

    #region BarrackPanel
    UnitNames _selectedUnitId;
    public void OnSelectUnitButtonClick(UnitNames unitId)
    {
        _selectedUnitId = unitId;
        SelectUnitPanel.ShowSelectUnitPanel();

        GetTMPro((int)Texts.UnitNameText).text = Language.GetBaseUnitName(unitId);
        if (Util.FindChild(Get<GameObject>((int)GameObjects.UnitPrefabPoint)) != null)
        {
            Managers.Resource.Destroy(Util.FindChild(Get<GameObject>((int)GameObjects.UnitPrefabPoint)));
        }
        GameObject obj = Managers.Resource.Instantiate($"Units/{unitId}_3", Get<GameObject>((int)GameObjects.UnitPrefabPoint).transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.identity;
        Util.FindChild<SortingGroup>(obj).sortingOrder = 9;

        string infoName,InfoValue;
        SetInfoText(unitId, out infoName, out InfoValue);
        GetTMPro((int)Texts.UnitInfoText).text = infoName;
        GetTMPro((int)Texts.UnitInfoText2).text = InfoValue;
    }

    public void OnUseUnitButtonClick(PointerEventData eventData)
    {
        Managers.UI.ShowPopupUI<UI_UseUnit>().Set((int)_selectedUnitId);
    }

    private void SetInfoText(UnitNames unitId, out string infoName, out string infoValue)
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
        SelectUnitPanel.HideSelectUnitPanel();
    }
    #endregion

    public void Clear()
    {
        Destroy(Get<GameObject>((int)GameObjects.NestedScrollManager).GetComponent<NestedScrollManager>());
        Destroy(Get<GameObject>((int)GameObjects.UnitSlotsScroll).GetComponent<ScrollScript>());
    }
}
