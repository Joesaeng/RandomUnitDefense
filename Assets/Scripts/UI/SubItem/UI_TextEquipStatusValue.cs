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

    public void SetValue(EquipItemStatus equipItemStatus)
    {
        float value = Managers.InGameItem.GetCurrentEquipedStatus(equipItemStatus);
        string valueText = " : ";
        if(equipItemStatus == EquipItemStatus.addedDamage)
        {
            valueText += (int)value;
        }
        else
        {
            valueText += (int)((value - 1) * 100) + "%";
            
        }
        GetComponent<TextMeshProUGUI>().text = valueText;
    }
}
