using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BtnChoiceFinger : UI_Base
{
    int _setIndex;
    int _selecteUnitId;
    public override void Init()
    {
        gameObject.AddUIEvent(OnChoiceButtonClicked);
    }
    public void Set(int index,int selectedUnitId)
    {
        _setIndex = index;
        _selecteUnitId = selectedUnitId;
    }
    public override void OnChangeLanguage()
    {
    }

    public void OnChoiceButtonClicked(PointerEventData eventData)
    {
        for(int index = 0; index < 5; ++index)
        {
            if (Managers.Player.Data.setUnits[index] == _selecteUnitId)
                Managers.Player.Data.setUnits[index] = 0;
        }
        Managers.Player.Data.setUnits[_setIndex] = _selecteUnitId;
        FindObjectOfType<UI_LobbyScene>().SetSetUnits();
        Managers.UI.ClosePopupUI();
    }
}
