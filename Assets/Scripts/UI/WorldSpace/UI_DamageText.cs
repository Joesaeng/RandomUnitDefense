using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_DamageText : UI_Base
{
    public override void Init()
    {
    }

    public void SetText(float damage,Vector3 pos)
    {
        transform.position = pos;
        string text = ((int)damage).ToString();
        GetComponentInChildren<TextMeshProUGUI>().text = text;
        StartCoroutine("CoDestroyThisObject");
    }

    IEnumerator CoDestroyThisObject()
    {
        yield return new WaitForSeconds(0.5f);
        Managers.Resource.Destroy(gameObject);
    }
}
