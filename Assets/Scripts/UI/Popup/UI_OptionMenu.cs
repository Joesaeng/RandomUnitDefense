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

        GetObject((int)OptionMenuObjects.BtnLanguage).AddUIEvent(OnLanguageButtonClicked);
        GetObject((int)OptionMenuObjects.BtnLanguage2).AddUIEvent(OnLanguageButtonClicked);
        GetObject((int)OptionMenuObjects.BtnResumeOrGoGame).AddUIEvent(OnResumeOrGoGameButtonClicked);

        GetObject((int)OptionMenuObjects.TextBtnLanguage).GetComponent<TextMeshProUGUI>().text = Language.LanguageButton;
        GetObject((int)OptionMenuObjects.TextBtnLanguage2).GetComponent<TextMeshProUGUI>().text = Language.LanguageText;
        GetObject((int)OptionMenuObjects.TextResumeOrGoGame).GetComponent<TextMeshProUGUI>().text = Language.ResumeOrGoGame;


    }

    private void OnResumeOrGoGameButtonClicked(PointerEventData data)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        switch (Managers.Game.CurrentScene)
        {
            case Define.Scene.Login:
                Managers.Scene.LoadSceneWithLoadingScene(Define.Scene.Lobby);
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

        GetObject((int)OptionMenuObjects.TextBtnLanguage).GetComponent<TextMeshProUGUI>().text = Language.LanguageButton;
        GetObject((int)OptionMenuObjects.TextBtnLanguage2).GetComponent<TextMeshProUGUI>().text = Language.LanguageText;
        GetObject((int)OptionMenuObjects.TextResumeOrGoGame).GetComponent<TextMeshProUGUI>().text = Language.ResumeOrGoGame;

    }
}
