using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SellAllUnits : UI_Popup
{
    enum Buttons
    {
        BtnCancel,
        BtnSell,
    }
    enum Texts
    {
        TextUnitDesc,
        TextSellPrices,
        TextBtnSell,
    }
    UnitNames _selectUnitId;
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        GetButton((int)Buttons.BtnCancel).gameObject.AddUIEvent(CancelButtonClicked);
        GetButton((int)Buttons.BtnSell).gameObject.AddUIEvent(SellButtonClicked);
    }
    public void Setup(UnitNames unitId)
    {
        _selectUnitId = unitId;
        int sellPrices = 0;
        List<Unit> foundUnits = Managers.Game.FindUnitsWithUnitId(unitId);
        foreach (Unit unit in foundUnits)
        {
            sellPrices += ConstantData.UnitSellingPrices[unit.Lv - 1];
        }
        GetTMPro((int)Texts.TextSellPrices).text = $"{sellPrices}";

        GetTMPro((int)Texts.TextUnitDesc).text = Language.SellAllUnit(unitId);
        GetTMPro((int)Texts.TextBtnSell).text = Language.Sell;
    }
    public void CancelButtonClicked(PointerEventData eventData)
    {
        ClosePopupUI();
    }
    public void SellButtonClicked(PointerEventData eventData)
    {
        Managers.Game.SellAllUnits(_selectUnitId);
        ClosePopupUI();
    }
    private void Start()
    {
        
    }
    private void Awake()
    {
        Init();
    }
}
