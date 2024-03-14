using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UseUnit : UI_Popup
{
    public int _selectedUnitId;
    enum GameObjects
    {
        PanelBtn,
    }
    enum Buttons
    {
        BtnCancel
    }
    enum Texts
    {
        TextCancel
    }

    public void Set(int selectedUnitId)
    {
        _selectedUnitId = selectedUnitId;
    }

    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));


        GetButton((int)Buttons.BtnCancel).gameObject.AddUIEvent(OnCancelButtonClicked);

        GetTMPro((int)Texts.TextCancel).text = Language.Cancel;

        Transform btnPanelTF = Get<GameObject>((int)GameObjects.PanelBtn).transform;
        int setUnits = ConstantData.SetUnitCount;
        for (int index = 0; index < setUnits; index++)
        {
            GameObject btnFinger = Managers.UI.MakeSubItem<UI_BtnChoiceFinger>(parent : btnPanelTF).gameObject;
            btnFinger.GetComponent<UI_BtnChoiceFinger>().Set(index, _selectedUnitId);
        }
    }

    public void OnCancelButtonClicked(PointerEventData eventData)
    {
        ClosePopupUI();
    }
}
