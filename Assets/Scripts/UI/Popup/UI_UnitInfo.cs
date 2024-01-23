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
    UnitStat_Base _unitStat;

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
        _unitStat = _unit.GetUnitStatus();
        SetInfo();

        transform.position = transform.parent.position + new Vector3(-2.5f,2,0f);
    }

    private void SetInfo()
    {
        GetTMPro((int)TMPros.TextName).text = Language.GetBaseUnitName(_unit.GetBaseUnit());
        GetTMPro((int)TMPros.TextLevel).text = $"{Language.GetUnitInfo(Language.UnitInfos.Level)} : {_unitStat.level}";
        GetTMPro((int)TMPros.TextInfo1).text = $"{Language.GetUnitInfo(Language.UnitInfos.AttackDamage)} : {_unitStat.attackDamage}";
        GetTMPro((int)TMPros.TextInfo2).text = $"{Language.GetUnitInfo(Language.UnitInfos.AttackRate)} : {_unitStat.attackRate}";
        GetTMPro((int)TMPros.TextInfo3).text = $"{Language.GetUnitInfo(Language.UnitInfos.AttackRange)} : {_unitStat.attackRange}";

        switch (_unit.GetBaseUnit())
        {
            case BaseUnits.Knight:
            case BaseUnits.Spearman:
            case BaseUnits.Archer:
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Textures/UnitIcon/Common");
                break;
            case BaseUnits.FireMagician:
            case BaseUnits.Viking:
            case BaseUnits.Warrior:
            {
                if (_unitStat is AOE unitStat)
                    GetTMPro((int)TMPros.TextInfo4).text = $"{Language.GetUnitInfo(Language.UnitInfos.WideAttackArea)} : {unitStat.wideAttackArea}";
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Textures/UnitIcon/AOE");
                break;
            }
            case BaseUnits.SlowMagician:
            {
                if (_unitStat is SlowMagician unitStat)
                {
                    GetTMPro((int)TMPros.TextInfo4).text = $"{Language.GetUnitInfo(Language.UnitInfos.WideAttackArea)} : {unitStat.wideAttackArea}";
                    GetTMPro((int)TMPros.TextInfo5).text = $"{Language.GetUnitInfo(Language.UnitInfos.SlowRatio)} : {unitStat.slowRatio}";
                    GetTMPro((int)TMPros.TextInfo6).text = $"{Language.GetUnitInfo(Language.UnitInfos.SlowDuration)} : {unitStat.slowDuration}";
                }
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Textures/UnitIcon/Debuffer");
                break;
            }
            case BaseUnits.StunGun:
            {
                if (_unitStat is StunGun unitStat)
                {
                    GetTMPro((int)TMPros.TextInfo4).text = $"{Language.GetUnitInfo(Language.UnitInfos.StunDuration)} : {unitStat.stunDuration}";
                }
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Textures/UnitIcon/Debuffer");
                break;
            }
            case BaseUnits.PoisonBowMan:
            {
                if (_unitStat is PoisonBowMan unitStat)
                {
                    GetTMPro((int)TMPros.TextInfo4).text = $"{Language.GetUnitInfo(Language.UnitInfos.PosionDamagePerSecond)} : {unitStat.poisonDamagePerSecond}/s";
                    GetTMPro((int)TMPros.TextInfo5).text = $"{Language.GetUnitInfo(Language.UnitInfos.PosionDuration)} : {unitStat.poisonDuration}";
                }
                GetImage((int)Images.TypeImage).sprite = Managers.Resource.Load<Sprite>($"Textures/UnitIcon/Debuffer");
                break;
            }
        }
    }
}
