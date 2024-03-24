using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_NotificationText : UI_Popup
{
    public void SetText(Define.NotiTexts noti)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = Language.GetNotiText(noti);
        gameObject.AddUIEvent(CloseUI);
    }
    public void CloseUI(PointerEventData eventData)
    {
        ClosePopupUI();
    }
}
