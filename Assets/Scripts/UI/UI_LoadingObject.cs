using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadingObject : UI_Base
{
    enum Images
    {
        LoadingGaugeFill
    }
    Image _fillGauge;
    enum Texts
    {
        LoadingText
    }
    private void Start()
    {
        
    }
    private void Awake()
    {
        Init();
    }
    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        _fillGauge = GetImage((int)Images.LoadingGaugeFill);
        ResetEx();
    }
    public void ResetEx()
    {
        _fillGauge.fillAmount = 0f;
        GetTMPro((int)Texts.LoadingText).text = Language.GetTipText();
    }

    public void UpdateFillGauge(float ratio)
    {
        _fillGauge.fillAmount = ratio;
    }

    public override void OnChangeLanguage()
    {
    }
}
