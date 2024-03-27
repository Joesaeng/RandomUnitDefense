using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SellAllRunes : UI_Popup
{
    enum GameObjects
    {
        PanelCheckSell
    }
    enum Buttons
    {
        BtnCancel1,
        BtnCancel2,
        BtnSell,
        BtnO,
        BtnX,
    }
    enum Texts
    {
        TextBtnSell,
        TextBtnCancel,
        TextWarning,
        TextSellPrices,
        TextCommon,
        TextRare,
        TextUnique,
        TextLegend,
        TextMyth,
    }
    enum Toggles
    {
        ToggleCommon,
        ToggleRare,
        ToggleUnique,
        ToggleLegend,
        ToggleMyth,
    }
    Dictionary<Data.GradeOfRune,Toggle> _toggleValueDict = new();
    UI_LobbyScene _scene;
    List<int> _sellRunesIndexs;
    int _sellPrices = 0;
    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Toggle>(typeof(Toggles));

        GetObject((int)GameObjects.PanelCheckSell).SetActive(false);
        // 버튼이벤트 바인드
        GetButton((int)Buttons.BtnCancel1).gameObject.AddUIEvent(CancelButtonClicked);
        GetButton((int)Buttons.BtnCancel2).gameObject.AddUIEvent(CancelButtonClicked);
        GetButton((int)Buttons.BtnX).gameObject.AddUIEvent(CancelButtonClicked);
        GetButton((int)Buttons.BtnSell).gameObject.AddUIEvent(SellButtonClicked);
        GetButton((int)Buttons.BtnO).gameObject.AddUIEvent(ConcentToSellButtonClicked);

        // 토글 초기화
        Toggle[] toggles = Gets<Toggle>();
        foreach (Toggle toggle in toggles)
        {
            toggle.isOn = false;
            GradeOfRune grade = Util.Parse<GradeOfRune>(toggle.name.RemovePrefix("Toggle"));
            _toggleValueDict.Add(grade, toggle);
        }

        // 텍스트 초기화
        TextMeshProUGUI[] texts = Gets<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in texts)
        {
            try
            {
                GradeOfRune grade = Util.Parse<GradeOfRune>(text.name.RemovePrefix("Text"));
                text.text = Language.GetRuneGradeText(grade);
            }
            catch { }
        }
        GetText((int)Texts.TextBtnSell).text = Language.Sell;
        GetText((int)Texts.TextBtnCancel).text = Language.Cancel;
        GetText((int)Texts.TextWarning).text = Language.SellWarning;
    }
    public void Set(UI_LobbyScene scene) => _scene = scene;

    public void CancelButtonClicked(PointerEventData eventData) => ClosePopupUI();
    public void SellButtonClicked(PointerEventData eventData)
    {
        GetObject((int)GameObjects.PanelCheckSell).SetActive(true);
        HashSet<GradeOfRune> selectGrade = new HashSet<GradeOfRune>();
        foreach (KeyValuePair<GradeOfRune, Toggle> kvp_gt in _toggleValueDict)
        {
            if (kvp_gt.Value.isOn == true)
                selectGrade.Add(kvp_gt.Key);
        }
        _sellRunesIndexs = Managers.Rune.FindRunesWithGradeOutIndexs(out _sellPrices, grades: selectGrade);
        GetText((int)Texts.TextSellPrices).text = $"{_sellPrices}";
    }
    public void ConcentToSellButtonClicked(PointerEventData eventData)
    {
        _scene.SellAllRunes(_sellRunesIndexs, _sellPrices);
        ClosePopupUI();
    }

    void Start()
    {

    }
    private void Awake()
    {
        Init();
    }
}
