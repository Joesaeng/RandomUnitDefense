using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class tempText : MonoBehaviour
{
    TextMeshProUGUI _text;
    float _curTime;
    string[] _texts = new string[]{"Loading.","Loading..", "Loading...", "Loading...."};
    int _curIndex = 0;
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _curTime = Time.realtimeSinceStartup;
        _curIndex = 0;
    }
    private void Update()
    {
        if (Time.realtimeSinceStartup - _curTime > 0.2f)
        {
            _text.text = _texts[_curIndex++];
            _curTime = Time.realtimeSinceStartup;
            _curIndex %= _texts.Length;
        }
    }
}
