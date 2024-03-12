using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class UI_TextEquipStatusName : UI_Base
{
    public override void Init()
    {
        
    }

    public override void OnChangeLanguage()
    {
    }

    public void SetUp(EquipItemStatus equipItemStatus)
    {
        GetComponent<TextMeshProUGUI>().text = Language.GetEquipStatusName(equipItemStatus);
    }
}
