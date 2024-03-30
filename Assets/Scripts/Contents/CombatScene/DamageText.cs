using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    WaitForSeconds restime = new WaitForSeconds(0.5f);
    TextMeshPro tmpro;
    Color criticalColor;
    Color posionColor;
    public void SetText(float damage, Vector3 pos, bool isCritical = false, bool isPoison = false)
    {

        transform.position = pos;
        string text = Util.ChangeNumber($"{(int)damage}");
        if (tmpro == null)
            tmpro = GetComponent<TextMeshPro>();
        tmpro.text = text;
        if (isCritical)
        {
            if(criticalColor != null)
                criticalColor = new Color(1f, 0.9f, 0f);
            tmpro.color = criticalColor;
        }
        else if (isPoison)
        {
            if (posionColor != null)
                posionColor = new Color(0f, 1f, 0.22f);
            tmpro.color = posionColor;
        }
        else
            tmpro.color = Color.white;

        StartCoroutine("CoDestroyDamageText");
    }
    IEnumerator CoDestroyDamageText()
    {
        yield return restime;
        Managers.Resource.Destroy(gameObject);
    }

}
