using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SetUnit : UI_Base
{
    enum Images
    {
        ImageUnit,
        ImageBack
    }
    public override void Init()
    {
        Bind<Image>(typeof(Images));
    }

    public override void OnChangeLanguage()
    {
    }

    public void SetImage(string unitname = "Art/UIImages/UI_nullUnit")
    {
        GetImage((int)Images.ImageUnit).enabled = true;

        GetImage((int)Images.ImageUnit).sprite = Managers.Resource.Load<Sprite>(unitname);
        if (unitname != "Art/UIImages/UI_nullUnit")
        {
            GetImage((int)Images.ImageUnit).rectTransform.localPosition = new Vector3(0f, 10f, 0f);
            GetImage((int)Images.ImageUnit).rectTransform.sizeDelta = new Vector2(250f, 250f);
        }
        else
        {
            GetImage((int)Images.ImageUnit).rectTransform.localPosition = Vector3.zero;
            GetImage((int)Images.ImageUnit).rectTransform.sizeDelta = new Vector2(100f, 100f);
        }
    }
    public void OffImage()
    {
        GetImage((int)Images.ImageUnit).enabled = false;
    }
    public void SelectImageOff()
    {
        GetImage((int)Images.ImageBack).enabled = false;
    }
    private void Start()
    {

    }
    private void Awake()
    {
        Init();
    }
}
