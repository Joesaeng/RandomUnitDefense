using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_NotificationText : UI_Base
{
    public override void Init()
    {
    }

    public override void OnChangeLanguage()
    {
    }

    public void SetText(Define.NotiTexts noti)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = Language.GetNotiText(noti);
        StartCoroutine("CloseUI");
    }
    IEnumerator CloseUI()
    {
        yield return YieldCache.WaitForSeconds(1.5f);
        Managers.Resource.Destroy(gameObject);
    }
}
