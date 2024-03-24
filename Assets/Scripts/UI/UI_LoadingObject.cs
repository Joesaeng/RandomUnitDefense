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
    public Image FillGauge;
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

        FillGauge = GetImage((int)Images.LoadingGaugeFill);
        ResetEx();
    }
    public void ResetEx()
    {
        FillGauge.fillAmount = 0f;
        GetTMPro((int)Texts.LoadingText).text = Language.GetTipText();
    }

    public override void OnChangeLanguage()
    {
    }
}
