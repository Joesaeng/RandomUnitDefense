using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UnitInfo : UI_Base
{
    Unit _unit;
    UnitStatus _unitStatus;

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
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(TMPros));
        _unit = transform.parent.GetComponent<Unit>();
        _unitStatus = Managers.UnitStatus.GetUnitStatus((UnitNames)_unit.ID, _unit.Lv);
        SetInfo();

        transform.position = transform.parent.position + new Vector3(-2.5f, 2, 0f);
    }

    private void SetInfo()
    {
        GetTMPro((int)TMPros.TextName).text = Language.GetBaseUnitName(_unitStatus.unit);
        GetTMPro((int)TMPros.TextLevel).text = $"{Language.GetUnitInfo(Language.UnitInfos.Level)} : {_unit.Lv}";
        GetTMPro((int)TMPros.TextInfo1).text = $"{Language.GetUnitInfo(Language.UnitInfos.AttackDamage)} : {_unitStatus.attackDamage}";
        GetTMPro((int)TMPros.TextInfo2).text = $"{Language.GetUnitInfo(Language.UnitInfos.AttackRate)} : {_unitStatus.attackRate}";
        GetTMPro((int)TMPros.TextInfo3).text = $"{Language.GetUnitInfo(Language.UnitInfos.AttackRange)} : {_unitStatus.attackRange}";

        switch (_unit.StateMachine.BaseUnit)
        {
            case UnitNames.Knight:
            case UnitNames.Spearman:
            case UnitNames.Archer:
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Textures/UnitIcon/Common");
                break;
            case UnitNames.FireMagician:
            case UnitNames.Viking:
            case UnitNames.Warrior:
            {
                GetTMPro((int)TMPros.TextInfo4).text = $"{Language.GetUnitInfo(Language.UnitInfos.WideAttackArea)} : {_unitStatus.wideAttackArea}";
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Textures/UnitIcon/AOE");
                break;
            }
            case UnitNames.SlowMagician:
            {
                GetTMPro((int)TMPros.TextInfo4).text = $"{Language.GetUnitInfo(Language.UnitInfos.WideAttackArea)} : {_unitStatus.wideAttackArea}";
                GetTMPro((int)TMPros.TextInfo5).text = $"{Language.GetUnitInfo(Language.UnitInfos.SlowRatio)} : {_unitStatus.debuffRatio}";
                GetTMPro((int)TMPros.TextInfo6).text = $"{Language.GetUnitInfo(Language.UnitInfos.SlowDuration)} : {_unitStatus.debuffDuration}";
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Textures/UnitIcon/Debuffer");
                break;
            }
            case UnitNames.StunGun:
            {
                GetTMPro((int)TMPros.TextInfo4).text = $"{Language.GetUnitInfo(Language.UnitInfos.StunDuration)} : {_unitStatus.debuffDuration}";
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Textures/UnitIcon/Debuffer");
                break;
            }
            case UnitNames.PoisonBowMan:
            {
                GetTMPro((int)TMPros.TextInfo4).text = $"{Language.GetUnitInfo(Language.UnitInfos.PosionDamagePerSecond)} : {_unitStatus.damagePerSecond}/s";
                GetTMPro((int)TMPros.TextInfo5).text = $"{Language.GetUnitInfo(Language.UnitInfos.PosionDuration)} : {_unitStatus.debuffDuration}";
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Textures/UnitIcon/Debuffer");
                break;
            }
        }
    }
}
