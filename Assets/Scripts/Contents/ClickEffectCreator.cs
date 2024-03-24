using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEffectCreator : MonoBehaviour
{
    float spawnsTime;
    float defaultTime = 0.05f;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && spawnsTime >= defaultTime)
        {
            CreateClickEffect();
            spawnsTime = 0f;
        }
        spawnsTime += Time.deltaTime;
    }

    void CreateClickEffect()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        Managers.Resource.Instantiate("ClickEffect", pos);
    }    
}
