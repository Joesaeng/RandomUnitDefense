using Data;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_CombatScene : UI_Scene
{
    enum GameObjects
    {
        PanelUpgrade,
        PanelItem,
        PanelEquipStatusName,
        PanelEquipStatusValue,
        PanelEquipedRunes,
        PanelUnitInfo,
        BlockerUpgrade,
        BlockerGamble
    }

    GameObject _panelItem;

    GameObject _panelUnitInfo;

    GameObject _panelEquipStatusName;
    GameObject _panelEquipStatusValue;
    List<UI_TextEquipStatusValue> _ui_textEquipStatusValues;

    List<UI_IngameItem> _ui_items;

    Animator _autoSkipAnim;
    Image _autoSkipImage;

    float _curseRuneValue = 0f;

    enum Buttons
    {
        BtnSpawn,
        BtnGamble,
        BtnEquipInfo,
        BtnPause,
        BtnGameSpeed,
        BtnSkip,
        BtnAutoSkip,
        BtnSell,
    }
    enum Texts
    {
        TextSpawn,
        TextGamble,
        TextGambleRuby,
        TextTheAmountOfRuby,
        TextStage,
        TextMonsterInfo,
        TextLeftTimeStage,
        TextChangeMonsterCount,
        TextFixedMonsterCount,
        TextEquipedRunesStatus,
        TextSkip,
        TextName,
        TextLevel,
        TextInfo1,
        TextInfo2,
        TextInfo3,
        TextInfo4,
        TextInfo5,
        TextInfo6,
        TextSellBtn,
    }

    enum Images
    {
        FillMonsterGageBar,
        ImageSpawnUnable,
        ImageGambleUnable,
        ImageGameSpeedValue,
        TypeImage
    }

    public override void Init()
    {
        base.Init();
        SetCanvasRenderModeCamera();
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.BtnSpawn).gameObject.AddUIEvent(OnSpawnButtonClicked);
        GetButton((int)Buttons.BtnGamble).gameObject.AddUIEvent(OnGambleButtonClicked);
        GetButton((int)Buttons.BtnEquipInfo).gameObject.AddUIEvent(OnEquipInfoButtonClicked);
        GetButton((int)Buttons.BtnGameSpeed).gameObject.AddUIEvent(OnGameSpeedButtonClicked);
        GetButton((int)Buttons.BtnPause).gameObject.AddUIEvent(OnPauseButtonClicked);
        GetButton((int)Buttons.BtnSkip).gameObject.AddUIEvent(OnSkipButtonClicked);
        GetButton((int)Buttons.BtnAutoSkip).gameObject.AddUIEvent(OnAutoSkipButtonClicked);

        GameObject autoSkipObj = GetButton((int)Buttons.BtnAutoSkip).gameObject;
        _autoSkipAnim = autoSkipObj.GetComponent<Animator>();
        _autoSkipAnim.enabled = Managers.Game.StageAutoSkip;

        _autoSkipImage = autoSkipObj.GetComponent<Image>();
        _autoSkipImage.sprite = Managers.Resource.Load<Sprite>("Art/UIImages/ButtonWhite");

        GetButton((int)Buttons.BtnSkip).gameObject.SetActive(false);
        GetText((int)Texts.TextSkip).enabled = false;
        GetText((int)Texts.TextFixedMonsterCount).text = $"{ConstantData.MonsterCountForGameOver}";
        GetText((int)Texts.TextSpawn).text = Language.Spawn;
        GetText((int)Texts.TextGamble).text = Language.GambleItem;

        // ���� �� �����Ӹ��� ȣ��Ǵ� ������Ʈ ĳ��
        _text_amountOfRuby = GetText((int)Texts.TextTheAmountOfRuby);
        _image_spawnUnable = GetImage((int)Images.ImageSpawnUnable);
        _image_gambleUnable = GetImage((int)Images.ImageGambleUnable);

        _text_stageTime = GetText((int)Texts.TextLeftTimeStage);
        _text_monsterCount = GetText((int)Texts.TextChangeMonsterCount);
        _image_monsterGageBar = GetImage((int)Images.FillMonsterGageBar);

        #region ��ư �̺�Ʈ ���ε�
        OnChangeAmountOfRuby(Managers.Game.Ruby);
        Managers.Game.OnChangedRuby -= OnChangeAmountOfRuby;
        Managers.Game.OnChangedRuby += OnChangeAmountOfRuby;

        OnNextStageEvent();
        Managers.Game.OnNextStage -= OnNextStageEvent;
        Managers.Game.OnNextStage += OnNextStageEvent;

        GetText((int)Texts.TextGambleRuby).text = $"<sprite=25> {ConstantData.BaseGambleCost}";
        Managers.InGameItem.OnGambleItem -= OnChangeItems;
        Managers.InGameItem.OnGambleItem += OnChangeItems;

        Managers.Game.OnSelectUnit -= OnSelectUnitEventReader;
        Managers.Game.OnSelectUnit += OnSelectUnitEventReader;

        Managers.Game.OnUnselectUnit -= OnUnselectUnitEventReader;
        Managers.Game.OnUnselectUnit += OnUnselectUnitEventReader;
        #endregion

        #region ���� ��� �������ͽ� �ʱ�ȭ

        _panelEquipStatusName = GetObject((int)GameObjects.PanelEquipStatusName);
        foreach (Transform child in _panelEquipStatusName.transform)
            Managers.Resource.Destroy(child.gameObject);

        _panelEquipStatusValue = GetObject((int)GameObjects.PanelEquipStatusValue);
        foreach (Transform child in _panelEquipStatusValue.transform)
            Managers.Resource.Destroy(child.gameObject);

        if (_ui_textEquipStatusValues != null)
            _ui_textEquipStatusValues.Clear();
        _ui_textEquipStatusValues = new List<UI_TextEquipStatusValue>();

        for (int equipStatus = 0; equipStatus < (int)Define.EquipItemStatus.Count; ++equipStatus)
        {
            GameObject equipStatusName = Managers.UI.MakeSubItem<UI_TextEquipStatusName>(parent : _panelEquipStatusName.transform).gameObject;
            GameObject equipStatusValue = Managers.UI.MakeSubItem<UI_TextEquipStatusValue>(parent : _panelEquipStatusValue.transform).gameObject;
            equipStatusName.transform.localScale = Vector3.one;
            equipStatusValue.transform.localScale = Vector3.one;
            equipStatusName.GetComponent<UI_TextEquipStatusName>().SetUp((EquipItemStatus)equipStatus);
            equipStatusValue.GetComponent<UI_TextEquipStatusValue>().SetValue((EquipItemStatus)equipStatus);
            _ui_textEquipStatusValues.Add(equipStatusValue.GetComponent<UI_TextEquipStatusValue>());
        }

        _panelEquipStatusName.SetActive(false);
        _panelEquipStatusValue.SetActive(false);

        #endregion

        #region ���â �ʱ�ȭ
        _panelItem = GetObject((int)GameObjects.PanelItem);
        foreach (Transform child in _panelItem.transform)
            Managers.Resource.Destroy(child.gameObject);

        if (_ui_items != null)
            _ui_items.Clear();
        _ui_items = new List<UI_IngameItem>();
        #endregion

        #region ���� ���׷��̵� �ʱ�ȭ
        GameObject panelUpgrade = GetObject((int)GameObjects.PanelUpgrade);
        //foreach (Transform child in panelUpgrade.transform)
        //    Managers.Resource.Destroy(child.gameObject);

        int unitCount = Managers.Game.SetUnits.Length;
        for(int i = 0; i < unitCount; i++)
        {
            // GameObject unitDesc = Managers.UI.MakeSubItem<UI_UnitDesc>(parent : panelUpgrade.transform).gameObject;
            GameObject unitDesc = panelUpgrade.transform.GetChild(i).gameObject;
            unitDesc.transform.localScale = Vector3.one;
            unitDesc.GetOrAddComponent<UI_UnitDesc>().SetInfo(i, Managers.Game.SetUnits[i]);
        }
        #endregion

        #region ������ �� ����
        for (int i = 0; i < Managers.Player.Data.EquipedRunes.Length; i++)
        {
            if (Managers.Player.Data.EquipedRunes[i] != null && 
                Managers.Player.Data.EquipedRunes[i].equipSlotIndex != -1)
            {
                GameObject equipedRune = Managers.UI.MakeSubItem<UI_EquipedRuneCombatScene>
                    (parent : GetObject((int)GameObjects.PanelEquipedRunes).transform).gameObject;
                equipedRune.transform.localScale = Vector3.one;
                equipedRune.GetComponent<UI_EquipedRuneCombatScene>().SetUp(i);
            }
        }
        GetObject((int)GameObjects.PanelEquipedRunes).AddUIEvent(OnEquipedRuneClick);
        GetText((int)Texts.TextEquipedRunesStatus).enabled = showEquipRuneStatus;
        {
            EquipedRuneStatus equipedRuneStatus = Managers.UnitStatus.RuneStatus;
            foreach(KeyValuePair<BaseRune,float> runestat in equipedRuneStatus.BaseRuneEffects)
            {
                GetText((int)Texts.TextEquipedRunesStatus).text += 
                    $"{Language.GetRuneBaseInfo(runestat.Key, runestat.Value)}\n";
            }
            foreach (KeyValuePair<AdditionalEffectName, float> runestat in equipedRuneStatus.AdditionalEffects)
            {
                GetText((int)Texts.TextEquipedRunesStatus).text +=
                    $"{Language.GetRuneAdditionalEffectText(runestat.Key, runestat.Value)}\n";
            }
        }
        GetText((int)Texts.TextEquipedRunesStatus).enabled = showEquipRuneStatus;

        // ���ַ�(���� ���)
        Managers.UnitStatus.RuneStatus.BaseRuneEffects.TryGetValue(BaseRune.Curse, out _curseRuneValue);

        #endregion

        #region UnitInfo ������Ʈ ����
        _panelUnitInfo = GetObject((int)GameObjects.PanelUnitInfo);
        _panelUnitInfo.SetActive(false);

        GetButton((int)Buttons.BtnSell).gameObject.AddUIEvent(OnSellButtonClicked);
        #endregion

    }
    TextMeshProUGUI _text_amountOfRuby;
    Image _image_spawnUnable;
    Image _image_gambleUnable;

    TextMeshProUGUI _text_stageTime;
    TextMeshProUGUI _text_monsterCount;
    Image _image_monsterGageBar;

    private void Update()
    {
        _text_stageTime.text = Managers.Time.GetStageTimeByTimeDisplayFormat(TimeManager.StageTimeType.LeftTime);
        _text_monsterCount.text = $"{Managers.Game.Monsters.Count}";
        _image_monsterGageBar.fillAmount =
            Util.CalculatePercent(Managers.Game.Monsters.Count, ConstantData.MonsterCountForGameOver);
    }

    public void OnNextStageEvent()
    {
        StageData stageData = Managers.Data.StageDict[Managers.Game.CurStage];
        string isBoss = stageData.isSpecial ? "<sprite=18>" : "";
        GetText((int)Texts.TextStage).text = $"{isBoss}STAGE {Managers.Game.CurStage}";
        string hp = stageData.monsterHp.ToString();
        hp = Util.ChangeNumber(hp);

        float reduceDefence = stageData.monsterdefense + (1 - _curseRuneValue);

        string defense = $"{(int)reduceDefence}";
        defense = Util.ChangeNumber(defense);
        GetText((int)Texts.TextMonsterInfo).text = 
            $"<sprite=46> : {hp}\n" +
            $"<sprite=50> : {defense}";

        if (Managers.Game.CurStage == 2)
            DeactiveUpgradeAndGambleBlocker();

        GetButton((int)Buttons.BtnSkip).gameObject.SetActive(false);
        GetText((int)Texts.TextSkip).enabled = false;
    }

    public void ActiveSkipButton()
    {
        GetButton((int)Buttons.BtnSkip).gameObject.SetActive(true);
        GetText((int)Texts.TextSkip).enabled = true;
    }

    private void DeactiveUpgradeAndGambleBlocker()
    {
        GetObject((int)GameObjects.BlockerGamble).SetActive(false);
        GetObject((int)GameObjects.BlockerUpgrade).SetActive(false);
    }

    public void OnSpawnButtonClicked(PointerEventData data)
    {
        Managers.Game.OnSpawnButtonClicked();
    }

    public void OnSkipButtonClicked(PointerEventData data)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        Managers.Time.SkipStage();
    }
    
    public void OnAutoSkipButtonClicked(PointerEventData data)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        Managers.Game.StageAutoSkip = !Managers.Game.StageAutoSkip;
        _autoSkipAnim.enabled = Managers.Game.StageAutoSkip;
        _autoSkipImage.sprite = Managers.Resource.Load<Sprite>("Art/UIImages/ButtonWhite");
    }

    public void OnGambleButtonClicked(PointerEventData data)
    {
        Managers.InGameItem.GambleItem();
    }

    bool showEquipRuneStatus = false;
    public void OnEquipedRuneClick(PointerEventData data)
    {
        showEquipRuneStatus = !showEquipRuneStatus;
        GetText((int)Texts.TextEquipedRunesStatus).enabled = showEquipRuneStatus;
    }

    public void OnGameSpeedButtonClicked(PointerEventData data)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        GetImage((int)Images.ImageGameSpeedValue).sprite =
            Managers.Resource.Load<Sprite>($"Art/UIImages/UI_{Managers.Time.ChangeTimeScale()}");
    }

    public void OnPauseButtonClicked(PointerEventData data)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        Managers.Time.GamePause();
        // �Ͻ����� UI Ű��
        Managers.UI.ShowPopupUI<UI_PauseMenu>();
    }

    public void OnChangeAmountOfRuby(int value)
    {
        _text_amountOfRuby.text = $"<sprite=25> {value}";
        _image_spawnUnable.enabled = value < ConstantData.RubyRequiredOneSpawnPlayerUnit;
        _image_gambleUnable.enabled = !Managers.InGameItem.CanGamble();
    }

    public void OnChangeItems(int value,InGameItemData itemdata)
    {
        GetText((int)Texts.TextGambleRuby).text = $"<sprite=25> {value}";
        if(itemdata != null)
        {
            GameObject item = Managers.UI.MakeSubItem<UI_IngameItem>(parent : _panelItem.transform).gameObject;
            item.transform.localScale = Vector3.one;
            UI_IngameItem ui_item = item.GetComponent<UI_IngameItem>();
            ui_item.SetInfo(itemdata);
            ui_item.OnClickedItemButton -= OnClickedItemButton;
            ui_item.OnClickedItemButton += OnClickedItemButton;
            _ui_items.Add(ui_item);
            Managers.Sound.Play(Define.SFXNames.GetTheItem);

            for (int equipStatus = 0; equipStatus < (int)EquipItemStatus.Count; ++equipStatus)
            {
                _ui_textEquipStatusValues[equipStatus].SetValue((EquipItemStatus)equipStatus);
            }
        }
    }

    public void OnClickedItemButton()
    {
        foreach(UI_IngameItem item in _ui_items)
        {
            item.DeactiveInfoObject();
        }
    }

    public void OnEquipInfoButtonClicked(PointerEventData data)
    {
        _panelEquipStatusName.SetActive(!_panelEquipStatusName.activeSelf);
        _panelEquipStatusValue.SetActive(!_panelEquipStatusValue.activeSelf);
    }

    public void OnSelectUnitEventReader(UnitNames id, int lv)
    {
        SetAndActiveUnitInfo(id, lv);
    }    

    public void OnUnselectUnitEventReader()
    {
        _panelUnitInfo.SetActive(false);
    }
    int _unitInfoSellCost = 0;
    public void SetAndActiveUnitInfo(UnitNames id, int lv)
    {
        _panelUnitInfo.SetActive(true);

        UnitStatus _unitStatus = Managers.UnitStatus.GetUnitStatus(id, lv);
        GetText((int)Texts.TextName).text = Language.GetBaseUnitName(_unitStatus.unit);
        GetText((int)Texts.TextLevel).text = $"{Language.GetUnitInfo(Language.UnitInfos.Level)} : {lv}";
        string damage = _unitStatus.attackDamage.ToString();
        damage = Util.ChangeNumber(damage);
        GetText((int)Texts.TextInfo1).text = $"{Language.GetUnitInfo(Language.UnitInfos.AttackDamage)} : {damage}";
        GetText((int)Texts.TextInfo2).text = $"{Language.GetUnitInfo(Language.UnitInfos.AttackRate)} : {_unitStatus.attackRate}";
        GetText((int)Texts.TextInfo3).text = $"{Language.GetUnitInfo(Language.UnitInfos.AttackRange)} : {_unitStatus.attackRange}";

        switch (id)
        {
            case UnitNames.Knight:
            case UnitNames.Spearman:
            case UnitNames.Archer:
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Art/UIImages/Common");
                break;
            case UnitNames.FireMagician:
            case UnitNames.Viking:
            case UnitNames.Warrior:
            {
                GetText((int)Texts.TextInfo4).text = $"{Language.GetUnitInfo(Language.UnitInfos.WideAttackArea)} : {_unitStatus.wideAttackArea}";
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Art/UIImages/AOE");
                break;
            }
            case UnitNames.SlowMagician:
            {
                GetText((int)Texts.TextInfo4).text = $"{Language.GetUnitInfo(Language.UnitInfos.WideAttackArea)} : {_unitStatus.wideAttackArea}";
                GetText((int)Texts.TextInfo5).text = $"{Language.GetUnitInfo(Language.UnitInfos.SlowRatio)} : {_unitStatus.debuffRatio}";
                GetText((int)Texts.TextInfo6).text = $"{Language.GetUnitInfo(Language.UnitInfos.SlowDuration)} : {_unitStatus.debuffDuration}";
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Art/UIImages/Debuffer");
                break;
            }
            case UnitNames.StunGun:
            {
                GetText((int)Texts.TextInfo4).text = $"{Language.GetUnitInfo(Language.UnitInfos.StunDuration)} : {_unitStatus.debuffDuration}";
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Art/UIImages/Debuffer");
                break;
            }
            case UnitNames.PoisonBowMan:
            {
                GetText((int)Texts.TextInfo4).text = $"{Language.GetUnitInfo(Language.UnitInfos.PosionDamagePerSecond)} : {_unitStatus.damagePerSecond}/s";
                GetText((int)Texts.TextInfo5).text = $"{Language.GetUnitInfo(Language.UnitInfos.PosionDuration)} : {_unitStatus.debuffDuration}";
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Art/UIImages/Debuffer");
                break;
            }
        }
        int[] sellCosts = ConstantData.UnitSellingPrices;
        _unitInfoSellCost = sellCosts[lv - 1];
        GetText((int)Texts.TextSellBtn).text = $"{Language.Sell} <sprite=25>{_unitInfoSellCost}";
    }

    public void OnSellButtonClicked(PointerEventData data)
    {
        Managers.Game.ClickedSellButton(_unitInfoSellCost);
    }

    public void Clear()
    {
        Managers.Game.OnChangedRuby -= OnChangeAmountOfRuby;

        Managers.Game.OnNextStage -= OnNextStageEvent;

        Managers.InGameItem.OnGambleItem -= OnChangeItems;

        Managers.Game.OnSelectUnit -= OnSelectUnitEventReader;

        Managers.Game.OnUnselectUnit -= OnUnselectUnitEventReader;
    }
}
