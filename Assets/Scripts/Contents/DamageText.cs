using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    float resTime;
    public void SetText(float damage, Vector3 pos)
    {
        transform.position = pos;
        string text = ((int)damage).ToString();
        GetComponent<TextMeshPro>().text = text;
        resTime = 0.5f;
    }

    private void Update()
    {
        if(resTime < 0)
            Managers.Resource.Destroy(gameObject);
        resTime -= Time.deltaTime;
    }
}
