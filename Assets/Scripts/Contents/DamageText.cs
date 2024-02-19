using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public void SetText(float damage, Vector3 pos)
    {
        transform.position = pos;
        string text = ((int)damage).ToString();
        GetComponent<TextMeshPro>().text = text;
        StartCoroutine("CoDestroyThisObject");
    }

    IEnumerator CoDestroyThisObject()
    {
        yield return new WaitForSeconds(0.5f);
        Managers.Resource.Destroy(gameObject);
    }
}
