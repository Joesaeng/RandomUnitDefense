using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_OptionMenu : UI_PauseMenu
{
    enum OptionMenuObjects
    {
        BtnLanguage,
        BtnLanguage2,
        BtnResumeOrGoGame,
        TextBtnLanguage,
        TextBtnLanguage2,
        TextResumeOrGoGame
    }


    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(OptionMenuObjects));

        Get<GameObject>((int)OptionMenuObjects.BtnLanguage).gameObject.AddUIEvent(OnLanguageButtonClicked);
        Get<GameObject>((int)OptionMenuObjects.BtnLanguage2).gameObject.AddUIEvent(OnLanguageButtonClicked);
        Get<GameObject>((int)OptionMenuObjects.BtnResumeOrGoGame).gameObject.AddUIEvent(OnResumeOrGoGameButtonClicked);

        Get<GameObject>((int)OptionMenuObjects.TextBtnLanguage).GetComponent<TextMeshProUGUI>().text = Language.LanguageButton;
        Get<GameObject>((int)OptionMenuObjects.TextBtnLanguage2).GetComponent<TextMeshProUGUI>().text = Language.LanguageText;
        Get<GameObject>((int)OptionMenuObjects.TextResumeOrGoGame).GetComponent<TextMeshProUGUI>().text = Language.ResumeOrGoGame;


    }

    private void OnResumeOrGoGameButtonClicked(PointerEventData data)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        switch (Managers.Game.CurrentScene)
        {
            case Define.Scene.Login:
                Managers.Scene.LoadScene(Define.Scene.Lobby);
                break;
            case Define.Scene.Lobby:
                ClosePopupUI();
                break;
        }
    }

    private void OnLanguageButtonClicked(PointerEventData data)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        Managers.Game.ChangeGameLanguage();
    }

    public override void OnChangeLanguage()
    {
        base.OnChangeLanguage();

        Get<GameObject>((int)OptionMenuObjects.TextBtnLanguage).GetComponent<TextMeshProUGUI>().text = Language.LanguageButton;
        Get<GameObject>((int)OptionMenuObjects.TextBtnLanguage2).GetComponent<TextMeshProUGUI>().text = Language.LanguageText;
        Get<GameObject>((int)OptionMenuObjects.TextResumeOrGoGame).GetComponent<TextMeshProUGUI>().text = Language.ResumeOrGoGame;

    }
}
