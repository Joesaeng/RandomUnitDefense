using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UnitDesc : UI_Base
{
    private void Start()
    {

    }
    private void Awake()
    {
        Init();
    }
    public int Slot { get; private set; }
    public UnitNames ID { get; private set; }
    enum Images
    {
        ImageUnit
    }
    enum Texts
    {
        TextUpgradeLevel,
        TextUpgradeCost,
        TextSellAll,
        TextDPS,
    }
    enum Buttons
    {
        BtnSellAll,
    }
    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));

        gameObject.AddUIEvent(ClickedUpgradeButton);
        GetButton((int)Buttons.BtnSellAll).gameObject.AddUIEvent(ClickedSellAllButton);
        GetText((int)Texts.TextSellAll).text = Language.SellAll;

        Managers.Game.OnDPSChecker -= SetDPSText;
        Managers.Game.OnDPSChecker += SetDPSText;
    }
    public void SetInfo(int slot, UnitNames id)
    {
        Slot = slot;
        ID = id;
        Image image = GetImage((int)Images.ImageUnit);
        image.sprite = Managers.Resource.Load<Sprite>($"Art/Units/{ID}");
        image.transform.localScale = Vector3.one * 2;
        SetText();
    }

    private void SetText()
    {
        GetText((int)Texts.TextUpgradeLevel).text = $"Lv.{Managers.UnitStatus.UnitUpgradLv[ID]}";
        GetText((int)Texts.TextUpgradeCost).text = $"<sprite=25> {Managers.Game.UpgradeCostOfUnits[Slot]}";
    }

    public void SetDPSText()
    {
        if (Managers.Game.UnitDPSDict.TryGetValue(ID, out Queue<int> dpsQ))
        {
            int[] dpss = dpsQ.ToArray();
            int dps = 0;
            for (int i = 0; i < dpss.Length; i++)
            {
                dps += dpss[i];
            }
            GetText((int)Texts.TextDPS).text = $"{Util.ChangeNumber(dps)}/s";
        }
    }

    public void ClickedUpgradeButton(PointerEventData data)
    {
        if (Managers.Game.CanUnitUpgrade(Slot))
        {
            Managers.Sound.Play(Define.SFXNames.UnitUpgrade);
            Managers.UnitStatus.ClickedUnitUpgrade(ID, Slot);
            SetText();
        }
    }

    public void ClickedSellAllButton(PointerEventData data)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        Managers.UI.ShowPopupUI<UI_SellAllUnits>().Setup(ID);
    }

    public override void OnChangeLanguage()
    {

    }
}
