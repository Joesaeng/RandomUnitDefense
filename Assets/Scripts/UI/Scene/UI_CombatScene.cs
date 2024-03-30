using Data;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
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
        BlockerUpgrade,
        BlockerGamble
    }

    GameObject _panelItem;

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
    }

    enum Images
    {
        FillMonsterGageBar,
        ImageSpawnUnable,
        ImageGambleUnable,
        ImageGameSpeedValue
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

        #region 버튼 이벤트 바인딩
        OnChangeAmountOfRuby(Managers.Game.Ruby);
        Managers.Game.OnChangedRuby -= OnChangeAmountOfRuby;
        Managers.Game.OnChangedRuby += OnChangeAmountOfRuby;

        OnNextStageEvent();
        Managers.Game.OnNextStage -= OnNextStageEvent;
        Managers.Game.OnNextStage += OnNextStageEvent;

        OnChangeItems(Managers.InGameItem.GambleCost);
        Managers.InGameItem.OnGambleItem -= OnChangeItems;
        Managers.InGameItem.OnGambleItem += OnChangeItems;
        #endregion

        #region 현재 장비 스테이터스 초기화

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

        #region 장비창 초기화
        _panelItem = GetObject((int)GameObjects.PanelItem);
        foreach (Transform child in _panelItem.transform)
            Managers.Resource.Destroy(child.gameObject);

        if (_ui_items != null)
            _ui_items.Clear();
        _ui_items = new List<UI_IngameItem>();
        #endregion

        #region 유닛 업그레이드 초기화
        GameObject panelUpgrade = GetObject((int)GameObjects.PanelUpgrade);
        foreach (Transform child in panelUpgrade.transform)
            Managers.Resource.Destroy(child.gameObject);

        int unitCount = Managers.Game.SetUnits.Length;
        for(int i = 0; i < unitCount; i++)
        {
            GameObject unitDesc = Managers.UI.MakeSubItem<UI_UnitDesc>(parent : panelUpgrade.transform).gameObject;
            unitDesc.transform.localScale = new Vector3(1f, 1f, 1f);
            unitDesc.GetComponent<UI_UnitDesc>().SetInfo(i, Managers.Game.SetUnits[i]);
        }
        #endregion

        #region 장착된 룬 세팅
        for (int i = 0; i < Managers.Player.Data.EquipedRunes.Length; i++)
        {
            if (Managers.Player.Data.EquipedRunes[i] != null && 
                Managers.Player.Data.EquipedRunes[i].equipSlotIndex != -1)
            {
                GameObject equipedRune = Managers.UI.MakeSubItem<UI_EquipedRuneCombatScene>
                    (parent : GetObject((int)GameObjects.PanelEquipedRunes).transform).gameObject;
                equipedRune.transform.localScale = new Vector3(1f, 1f, 1f);
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

        // 저주룬(몬스터 방깎)
        Managers.UnitStatus.RuneStatus.BaseRuneEffects.TryGetValue(BaseRune.Curse, out _curseRuneValue);

        #endregion

        _text_stageTime = GetText((int)Texts.TextLeftTimeStage);
        _text_monsterCount = GetText((int)Texts.TextChangeMonsterCount);
        _image_monsterGageBar = GetImage((int)Images.FillMonsterGageBar);
    }
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
        // 일시정지 UI 키기
        Managers.UI.ShowPopupUI<UI_PauseMenu>();
    }

    public void OnChangeAmountOfRuby(int value)
    {
        GetText((int)Texts.TextTheAmountOfRuby).text = $"<sprite=25> {value}";
        GetImage((int)Images.ImageSpawnUnable).enabled = value < ConstantData.RubyRequiredOneSpawnPlayerUnit;
        GetImage((int)Images.ImageGambleUnable).enabled = !Managers.InGameItem.CanGamble();

    }

    public void OnChangeItems(int value,InGameItemData itemdata = null)
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

    public void Clear()
    {
        Managers.Game.OnChangedRuby -= OnChangeAmountOfRuby;

        Managers.Game.OnNextStage -= OnNextStageEvent;

        Managers.InGameItem.OnGambleItem -= OnChangeItems;
    }
}
