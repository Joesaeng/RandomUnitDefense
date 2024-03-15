using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    WaitForSeconds restime = new WaitForSeconds(0.5f);
    TextMeshPro tmpro;
    public void SetText(float damage, Vector3 pos)
    {
        transform.position = pos;
        string text = ((int)damage).ToString();
        if(tmpro == null)
            tmpro = GetComponent<TextMeshPro>();
        tmpro.text = text;
        StartCoroutine("CoDestroyDamageText");
    }
    IEnumerator CoDestroyDamageText()
    {
        yield return restime;
        Managers.Resource.Destroy(gameObject);
    }

}
