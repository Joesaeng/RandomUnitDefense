using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PauseMenu : UI_Popup
{
    enum Buttons
    {
        BtnBGM,
        BtnSFX,
        BtnLobby,
        BtnResume,
    }

    enum TMPros
    {
        TextBGM,
        TextSFX,
        TextLobby,
        TextResume,
    }

    enum Sliders
    {
        SliderBGM,
        SliderSFX,
    }

    enum Images
    {
        BtnBGM,
        BtnSFX,
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TMPros));
        Bind<Slider>(typeof(Sliders));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.BtnBGM).gameObject.AddUIEvent(OnBGMButtonClicked);
        GetButton((int)Buttons.BtnSFX).gameObject.AddUIEvent(OnSFXButtonClicked);
        GetButton((int)Buttons.BtnLobby).gameObject.AddUIEvent(OnLobbyButtonClicked);
        GetButton((int)Buttons.BtnResume).gameObject.AddUIEvent(OnResumeButtonClicked);

        GetTMPro((int)TMPros.TextBGM).text = Language.BGM;
        GetTMPro((int)TMPros.TextSFX).text = Language.SFX;
        GetTMPro((int)TMPros.TextLobby).text = Language.ExitToLobby;
        GetTMPro((int)TMPros.TextResume).text = Language.ResumeToGame;

        Get<Slider>((int)Sliders.SliderBGM).value = Managers.Sound.BGMVolume;
        Get<Slider>((int)Sliders.SliderSFX).value = Managers.Sound.SFXVolume;

        _prevBGMVolume = Managers.Sound.BGMVolume;
        _isOnBGM = _prevBGMVolume > 0;
        _prevSFXVolume = Managers.Sound.SFXVolume;
        _isOnSFX = _prevSFXVolume > 0;

        GetImage((int)Images.BtnBGM).sprite =
            Managers.Resource.Load<Sprite>($"Art/UIImages/UI_{_isOnBGM}");
        GetImage((int)Images.BtnSFX).sprite =
            Managers.Resource.Load<Sprite>($"Art/UIImages/UI_{_isOnSFX}");

        Get<Slider>((int)Sliders.SliderBGM).onValueChanged.AddListener(OnBGMChanged);
        Get<Slider>((int)Sliders.SliderSFX).onValueChanged.AddListener(OnSFXChanged);
    }

    public override void OnChangeLanguage()
    {
        base.OnChangeLanguage();

        GetTMPro((int)TMPros.TextBGM).text = Language.BGM;
        GetTMPro((int)TMPros.TextSFX).text = Language.SFX;
        GetTMPro((int)TMPros.TextLobby).text = Language.ExitToLobby;
        GetTMPro((int)TMPros.TextResume).text = Language.ResumeToGame;
    }

    private void OnBGMButtonClicked(PointerEventData data)
    {
        Managers.Sound.Play("Click");
        if(_isOnBGM == true)
        {
            _isOnBGM = false;
            Get<Slider>((int)Sliders.SliderBGM).value = 0;
        }
        else
        {
            _isOnBGM = true;
            if (_prevBGMVolume <= 0)
                _prevBGMVolume = 0.5f;
            Get<Slider>((int)Sliders.SliderBGM).value = _prevBGMVolume;
        }
        GetImage((int)Images.BtnBGM).sprite =
                Managers.Resource.Load<Sprite>($"Art/UIImages/UI_{_isOnBGM}");
    }

    private void OnSFXButtonClicked(PointerEventData data)
    {
        Managers.Sound.Play("Click");
        if (_isOnSFX == true)
        {
            _isOnSFX = false;
            Get<Slider>((int)Sliders.SliderSFX).value = 0;
        }
        else
        {
            _isOnSFX = true;
            if (_prevSFXVolume <= 0)
                _prevSFXVolume = 0.5f;
            Get<Slider>((int)Sliders.SliderSFX).value = _prevSFXVolume;
        }
        GetImage((int)Images.BtnSFX).sprite =
                Managers.Resource.Load<Sprite>($"Art/UIImages/UI_{_isOnSFX}");
    }

    private void OnLobbyButtonClicked(PointerEventData data)
    {
        Managers.Sound.Play("Click");

    }

    private void OnResumeButtonClicked(PointerEventData data)
    {
        Managers.Sound.Play("Click");
        Managers.UI.ClosePopupUI(this);
        Managers.Time.GameResume();
    }

    bool _isOnBGM;
    float _prevBGMVolume;
    private void OnBGMChanged(float value)
    {
        _isOnBGM = value > 0;
        // SoundManager에서 배경음악 볼륨 조절
        Managers.Sound.BGMVolume = value;
        if(value > 0.01f)
            _prevBGMVolume = value;
        GetImage((int)Images.BtnBGM).sprite =
            Managers.Resource.Load<Sprite>($"Art/UIImages/UI_{_isOnBGM}");
        Managers.Sound.ChangeBGMVolume();
    }

    bool _isOnSFX;
    float _prevSFXVolume;
    private void OnSFXChanged(float value)
    {
        _isOnSFX = value > 0;
        // SoundManager에서 효과음 볼륨 조절
        Managers.Sound.SFXVolume = value;
        if (value > 0.01f)
            _prevSFXVolume = value;
        GetImage((int)Images.BtnSFX).sprite =
            Managers.Resource.Load<Sprite>($"Art/UIImages/UI_{_isOnSFX}");
    }
}
