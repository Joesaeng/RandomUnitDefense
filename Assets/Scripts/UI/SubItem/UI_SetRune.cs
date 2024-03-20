using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SetRune : UI_Base
{
    enum Images
    {
        RuneImage
    }
    enum Texts
    {
        RuneText
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        GetImage((int)Images.RuneImage).enabled = false;
        GetTMPro((int)Texts.RuneText).enabled = false;
    }

    public void SetRuneImage(BaseRune baseRune, GradeOfRune gradeOfRune)
    {
        GetImage((int)Images.RuneImage).sprite = Managers.Rune.RuneSprites[gradeOfRune];
        GetTMPro((int)Texts.RuneText).text = Managers.Rune.RuneTextImages[baseRune];
        GetImage((int)Images.RuneImage).enabled = true;
        GetTMPro((int)Texts.RuneText).enabled = true;
    }
    public void OffImage()
    {
        GetImage((int)Images.RuneImage).sprite = null;
        GetTMPro((int)Texts.RuneText).text = null;
        GetImage((int)Images.RuneImage).enabled = false;
        GetTMPro((int)Texts.RuneText).enabled = false;
    }

    public override void OnChangeLanguage()
    {
    }

    private void Start()
    {

    }
    private void Awake()
    {
        Init();
    }
}
