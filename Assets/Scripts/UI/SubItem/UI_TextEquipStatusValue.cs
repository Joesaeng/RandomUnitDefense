using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class UI_TextEquipStatusValue : UI_Base
{
    public override void Init()
    {
    }

    public override void OnChangeLanguage()
    {
    }

    public void SetValue(EquipItemStatus equipItemStatus)
    {
        float value = Managers.InGameItem.GetCurrentEquipedStatus(equipItemStatus);
        string valueText = " : ";
        if(equipItemStatus == EquipItemStatus.addedDamage
            || equipItemStatus == EquipItemStatus.increaseAttackRange)
        {
            valueText += value;
        }
        else
        {
            valueText += (Mathf.Round((value - 1) * 100)) + "%";
            
        }
        GetComponent<TextMeshProUGUI>().text = valueText;
    }
}
