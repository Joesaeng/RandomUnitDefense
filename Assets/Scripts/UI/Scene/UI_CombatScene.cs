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
    }

    GameObject _panelItem;

    GameObject _panelEquipStatusName;
    GameObject _panelEquipStatusValue;
    List<UI_TextEquipStatusValue> _ui_textEquipStatusValues;

    List<UI_Item> _ui_items;

    enum Buttons
    {
        BtnSpawn,
        BtnGamble,
        BtnEquipInfo,
        BtnPause,
        BtnGameSpeed,
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
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.BtnSpawn).gameObject.AddUIEvent(OnSpawnButtonClicked);
        GetButton((int)Buttons.BtnGamble).gameObject.AddUIEvent(OnGambleButtonClicked);
        GetButton((int)Buttons.BtnEquipInfo).gameObject.AddUIEvent(OnEquipInfoButtonClicked);
        GetButton((int)Buttons.BtnGameSpeed).gameObject.AddUIEvent(OnGameSpeedButtonClicked);
        GetButton((int)Buttons.BtnPause).gameObject.AddUIEvent(OnPauseButtonClicked);
        GetTMPro((int)Texts.TextFixedMonsterCount).text = $"{ConstantData.MonsterCountForGameOver}";
        GetTMPro((int)Texts.TextSpawn).text = Language.Spawn;
        GetTMPro((int)Texts.TextGamble).text = Language.GambleItem;

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

        _panelEquipStatusName = Get<GameObject>((int)GameObjects.PanelEquipStatusName);
        foreach (Transform child in _panelEquipStatusName.transform)
            Managers.Resource.Destroy(child.gameObject);

        _panelEquipStatusValue = Get<GameObject>((int)GameObjects.PanelEquipStatusValue);
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
        _panelItem = Get<GameObject>((int)GameObjects.PanelItem);
        foreach (Transform child in _panelItem.transform)
            Managers.Resource.Destroy(child.gameObject);

        if (_ui_items != null)
            _ui_items.Clear();
        _ui_items = new List<UI_Item>();
        #endregion

        #region 유닛 업그레이드 초기화
        GameObject panelUpgrade = Get<GameObject>((int)GameObjects.PanelUpgrade);
        foreach (Transform child in panelUpgrade.transform)
            Managers.Resource.Destroy(child.gameObject);

        int unitCount = Managers.Game.SetUnits.Length;
        for(int i = 0; i < unitCount; i++)
        {
            GameObject upgradeBtn = Managers.UI.MakeSubItem<UI_BtnUpgrade>(parent : panelUpgrade.transform).gameObject;
            upgradeBtn.transform.localScale = new Vector3(1f, 1f, 1f);
            upgradeBtn.GetComponent<UI_BtnUpgrade>().SetInfo(i, Managers.Game.SetUnits[i]);
        }
        #endregion

        _text_stageTime = GetTMPro((int)Texts.TextLeftTimeStage);
        _text_monsterCount = GetTMPro((int)Texts.TextChangeMonsterCount);
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
        GetTMPro((int)Texts.TextStage).text = $"STAGE {Managers.Game.CurStage}";
        MonsterData monsterData = Managers.Data.GetMonsterData(Managers.Game.CurStage);
        string hp = monsterData.maxHp.ToString();
        hp = Util.ChangeNumber(hp);
        string defense = monsterData.defense.ToString();
        defense = Util.ChangeNumber(defense);
        GetTMPro((int)Texts.TextMonsterInfo).text = 
            $"<sprite=46> : {hp}\n" +
            $"<sprite=50> : {defense}";

    }

    public void OnSpawnButtonClicked(PointerEventData data)
    {
        Managers.Game.OnSpawnButtonClicked();
    }

    public void OnGambleButtonClicked(PointerEventData data)
    {
        Managers.InGameItem.GambleItem();
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
        GetTMPro((int)Texts.TextTheAmountOfRuby).text = $"<sprite=25> {value}";
        GetImage((int)Images.ImageSpawnUnable).enabled = value <= ConstantData.RubyRequiredOneSpawnPlayerUnit;
        GetImage((int)Images.ImageGambleUnable).enabled = Managers.InGameItem.CanGamble();

    }

    public void OnChangeItems(int value,InGameItemData itemdata = null)
    {
        GetTMPro((int)Texts.TextGambleRuby).text = $"<sprite=25> {value}";
        if(itemdata != null)
        {
            GameObject item = Managers.UI.MakeSubItem<UI_Item>(parent : _panelItem.transform).gameObject;
            item.transform.localScale = Vector3.one;
            UI_Item ui_item = item.GetComponent<UI_Item>();
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
        foreach(UI_Item item in _ui_items)
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
