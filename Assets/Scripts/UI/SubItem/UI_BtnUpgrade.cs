using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BtnUpgrade : UI_Base
{
    private void Start()
    {
        
    }
    private void Awake()
    {
        Init();
    }
    public int Slot {  get; private set; }
    public UnitNames ID {  get; private set; }
    enum Images
    {
        ImageUnit
    }
    enum TMPros
    {
        TextUpgradeLevel,
        TextUpgradeCost,
    }
    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(TMPros));
        Bind<Image>(typeof(Images));

        gameObject.AddUIEvent(ClickedUpgradeButton);
    }
    public void SetInfo(int slot, int id)
    {
        Slot = slot;
        ID = (UnitNames)id;
        SetText();
    }

    private void SetText()
    {
        GetTMPro((int)TMPros.TextUpgradeLevel).text = $"Lv.{Managers.UnitStatus.UnitUpgradLv[ID]}";
        GetTMPro((int)TMPros.TextUpgradeCost).text = $"<sprite=25> {Managers.Game._upgradeCostOfUnits[Slot]}";
    }

    public void ClickedUpgradeButton(PointerEventData data)
    {
        if(Managers.Game.CanUnitUpgrade(Slot))
        {
            Managers.UnitStatus.ClickedUnitUpgrade(ID, Slot);
            SetText();
        }
    }
}
