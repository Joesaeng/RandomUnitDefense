using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempCanvas : MonoBehaviour
{
    void Start()
    {
        CanvasScaler cs = GetComponent<CanvasScaler>();
        Debug.Log(cs.uiScaleMode);
        Debug.Log(cs.referenceResolution);
        Debug.Log(cs.referenceResolution.x);
        Debug.Log(cs.referenceResolution.y);
    }

    void Update()
    {
        
    }
}
