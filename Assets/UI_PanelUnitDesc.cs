using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PanelUnitDesc : UI_Base
{
    enum Buttons
    {
        UnitDesc0, UnitDesc1, UnitDesc2, UnitDesc3, UnitDesc4,
        BtnSellAll0, BtnSellAll1, BtnSellAll2, BtnSellAll3, BtnSellAll4
    }
    enum Images
    {
        ImageUnit0, ImageUnit1, ImageUnit2, ImageUnit3, ImageUnit4,
    }
    enum Texts
    {
        TextSellAll0, TextSellAll1, TextSellAll2, TextSellAll3, TextSellAll4,
        TextUpgradeLevel0, TextUpgradeLevel1, TextUpgradeLevel2, TextUpgradeLevel3, TextUpgradeLevel4,
        TextUpgradeCost0, TextUpgradeCost1, TextUpgradeCost2, TextUpgradeCost3, TextUpgradeCost4,
        TextDPS0, TextDPS1, TextDPS2, TextDPS3, TextDPS4,
    }

    TextMeshProUGUI[] _dpsTexts;

    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));

        int unitCount = Managers.Game.SetUnits.Length;
        _dpsTexts = new TextMeshProUGUI[unitCount];

        for(int i = 0; i < unitCount; i++)
        {
            _dpsTexts[i] = GetText((int)Util.Parse<Texts>($"TextDPS{i}"));
        }

        string sellAll = Language.SellAll;
        for (int i = 0; i < unitCount; i++)
        {
            GetText((int)Util.Parse<Texts>($"TextSellAll{i}")).text = sellAll;
        }

        UnitNames[] unitNames = Managers.Game.SetUnits;
        for (int i = 0; i < unitCount; i++)
        {
            SetDesc(i, unitNames[i]);
        }

        Managers.Game.OnDPSChecker -= SetDPSText;
        Managers.Game.OnDPSChecker += SetDPSText;
    }

    private void SetDesc(int slot, UnitNames id)
    {
        GetButton((int)Util.Parse<Buttons>($"UnitDesc{slot}")).onClick.AddListener(() => { UnitUpgrade(slot); }) ;
        GetButton((int)Util.Parse<Buttons>($"BtnSellAll{slot}")).onClick.AddListener(() => { SellAll(id); }) ;
        Image image = GetImage((int)Util.Parse<Images>($"ImageUnit{slot}"));
        image.sprite = Managers.Resource.Load<Sprite>($"Art/Units/{id}");
        image.transform.localScale = Vector3.one * 2;
        SetText(slot,id);
    }

    private void SetText(int slot, UnitNames id)
    {
        GetText((int)Util.Parse<Texts>($"TextUpgradeLevel{slot}")).text = $"Lv.{Managers.UnitStatus.UnitUpgradLv[id]}";
        GetText((int)Util.Parse<Texts>($"TextUpgradeCost{slot}")).text = $"<sprite=25> {Managers.Game.UpgradeCostOfUnits[slot]}";
    }

    public void UnitUpgrade(int slot)
    {
        if (Managers.Game.CanUnitUpgrade(slot))
        {
            Managers.Sound.Play(Define.SFXNames.UnitUpgrade);
            UnitNames id = Managers.Game.SetUnits[slot];
            Managers.UnitStatus.ClickedUnitUpgrade(id, slot);
            SetText(slot, id);
        }
    }

    public void SellAll(UnitNames id)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        Managers.UI.ShowPopupUI<UI_SellAllUnits>().Setup(id);
    }

    public void SetDPSText()
    {
        for(int i = 0; i < 5; ++i)
        {
            UnitNames slotUnitID = Managers.Game.SetUnits[i];
            if (Managers.Game.UnitDPSDict.TryGetValue(slotUnitID, out int dps))
            {
                _dpsTexts[i].text = $"{Util.ChangeNumber(dps)}/s";
            }
        }
        
    }

    public override void OnChangeLanguage()
    {
    }
}
