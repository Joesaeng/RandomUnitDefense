using Data;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UnitInfo : UI_Base
{
    Unit _unit;
    UnitStatus _unitStatus;
    enum Buttons
    {
        BtnSell
    }
    enum Images
    {
        TypeImage,
    }
    enum TMPros
    {
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

    int sellCost;

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(TMPros));
        _unit = transform.parent.GetComponent<Unit>();
        _unitStatus = Managers.UnitStatus.GetUnitStatus((UnitNames)_unit.ID, _unit.Lv);
        SetInfo();

        transform.position = transform.parent.position + new Vector3(-2.5f, 2, 0f);

        GetButton((int)Buttons.BtnSell).gameObject.AddUIEvent(ClickedSellButton);
    }

    private void SetInfo()
    {
        GetTMPro((int)TMPros.TextName).text = Language.GetBaseUnitName(_unitStatus.unit);
        GetTMPro((int)TMPros.TextLevel).text = $"{Language.GetUnitInfo(Language.UnitInfos.Level)} : {_unit.Lv}";
        string damage = _unitStatus.attackDamage.ToString();
        damage = Util.ChangeNumber(damage);
        GetTMPro((int)TMPros.TextInfo1).text = $"{Language.GetUnitInfo(Language.UnitInfos.AttackDamage)} : {damage}";
        GetTMPro((int)TMPros.TextInfo2).text = $"{Language.GetUnitInfo(Language.UnitInfos.AttackRate)} : {_unitStatus.attackRate}";
        GetTMPro((int)TMPros.TextInfo3).text = $"{Language.GetUnitInfo(Language.UnitInfos.AttackRange)} : {_unitStatus.attackRange}";

        switch (_unit.StateMachine.BaseUnit)
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
                GetTMPro((int)TMPros.TextInfo4).text = $"{Language.GetUnitInfo(Language.UnitInfos.WideAttackArea)} : {_unitStatus.wideAttackArea}";
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Art/UIImages/AOE");
                break;
            }
            case UnitNames.SlowMagician:
            {
                GetTMPro((int)TMPros.TextInfo4).text = $"{Language.GetUnitInfo(Language.UnitInfos.WideAttackArea)} : {_unitStatus.wideAttackArea}";
                GetTMPro((int)TMPros.TextInfo5).text = $"{Language.GetUnitInfo(Language.UnitInfos.SlowRatio)} : {_unitStatus.debuffRatio}";
                GetTMPro((int)TMPros.TextInfo6).text = $"{Language.GetUnitInfo(Language.UnitInfos.SlowDuration)} : {_unitStatus.debuffDuration}";
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Art/UIImages/Debuffer");
                break;
            }
            case UnitNames.StunGun:
            {
                GetTMPro((int)TMPros.TextInfo4).text = $"{Language.GetUnitInfo(Language.UnitInfos.StunDuration)} : {_unitStatus.debuffDuration}";
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Art/UIImages/Debuffer");
                break;
            }
            case UnitNames.PoisonBowMan:
            {
                GetTMPro((int)TMPros.TextInfo4).text = $"{Language.GetUnitInfo(Language.UnitInfos.PosionDamagePerSecond)} : {_unitStatus.damagePerSecond}/s";
                GetTMPro((int)TMPros.TextInfo5).text = $"{Language.GetUnitInfo(Language.UnitInfos.PosionDuration)} : {_unitStatus.debuffDuration}";
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Art/UIImages/Debuffer");
                break;
            }
        }
        int[] sellCosts = ConstantData.UnitSellingPrices;
        sellCost = sellCosts[_unit.Lv - 1];
        GetTMPro((int)TMPros.TextSellBtn).text = $"{Language.Sell} <sprite=25>{sellCost}";
    }

    private void ClickedSellButton(PointerEventData data)
    {
        Managers.Game.ClickedSellButton(sellCost);
    }

    public override void OnChangeLanguage()
    {
    }
}
