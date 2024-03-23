using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BtnChoiceFinger : UI_Base
{
    int _setIndex;
    int _selectedUnitId;
    Rune _selectedRune;
    public override void Init()
    {
        gameObject.AddUIEvent(OnChoiceButtonClicked);
    }
    public void Set(int index, int selectedUnitId)
    {
        _setIndex = index;
        _selectedUnitId = selectedUnitId;
        _selectedRune = null;
    }
    public void Set(int index, Rune rune)
    {
        _setIndex = index;
        _selectedRune = rune;
        _selectedUnitId = 9999;
    }
    public override void OnChangeLanguage()
    {
    }

    public void OnChoiceButtonClicked(PointerEventData eventData)
    {
        PlayerData playerData = Managers.Player.Data;
        if (_selectedRune == null) // 유닛 선택용 스크립트
        {
            for (int index = 0; index < ConstantData.SetUnitCount; ++index)
            {
                if (playerData.setUnits[index] == _selectedUnitId)
                    playerData.setUnits[index] = 0;
            }
            playerData.setUnits[_setIndex] = _selectedUnitId;
            FindObjectOfType<UI_LobbyScene>().SetSetUnits();
            Managers.UI.ClosePopupUI();
        }
        else
        {
            if (playerData.EquipedRunes[_setIndex] != null)
            {
                playerData.EquipedRunes[_setIndex].isEquip = false;
                playerData.EquipedRunes[_setIndex].equipSlotIndex = -1;
            }
            for (int index = 0; index < playerData.EquipedRunes.Length; ++index)
            {
                if (playerData.EquipedRunes[index] == _selectedRune)
                {
                    playerData.EquipedRunes[index] = null;
                }
            }
            playerData.EquipedRunes[_setIndex] = _selectedRune;
            playerData.EquipedRunes[_setIndex].isEquip = true;
            playerData.EquipedRunes[_setIndex].equipSlotIndex = _setIndex;
            FindObjectOfType<UI_LobbyScene>().SetEquipedRunes();
            Managers.UI.ClosePopupUI();
        }
    }
}
