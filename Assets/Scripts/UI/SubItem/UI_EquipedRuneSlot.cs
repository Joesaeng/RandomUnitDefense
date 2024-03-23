using Data;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipedRuneSlot : UI_Base
{
    Rune _rune;
    UI_LobbyScene _ui_LobbyScene;

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

        gameObject.AddUIEvent(ClickedEquipedRuneSlot);
    }

    public void SetRune(Rune rune, UI_LobbyScene uI_LobbyScene)
    {
        _rune = rune;
        _ui_LobbyScene = uI_LobbyScene;
        GetImage((int)Images.RuneImage).sprite = Managers.Rune.RuneSprites[rune.gradeOfRune];
        GetTMPro((int)Texts.RuneText).text = Managers.Rune.RuneTextImages[rune.baseRune];
        GetImage((int)Images.RuneImage).enabled = true;
        GetTMPro((int)Texts.RuneText).enabled = true;
    }
    public void OffImage()
    {
        _rune = null;
        GetImage((int)Images.RuneImage).sprite = null;
        GetTMPro((int)Texts.RuneText).text = null;
        GetImage((int)Images.RuneImage).enabled = false;
        GetTMPro((int)Texts.RuneText).enabled = false;
    }

    public void ClickedEquipedRuneSlot(PointerEventData data)
    {
        if (_rune == null)
            return;
        _ui_LobbyScene.UpdateSelectedRunePanel(_rune);
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
