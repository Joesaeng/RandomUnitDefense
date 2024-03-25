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
        RuneImage,
        RuneTextImage,
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));

        GetImage((int)Images.RuneImage).enabled = false;
        GetImage((int)Images.RuneTextImage).enabled = false;

        gameObject.AddUIEvent(ClickedEquipedRuneSlot);
    }

    public void SetRune(Rune rune, UI_LobbyScene uI_LobbyScene)
    {
        _rune = rune;
        _ui_LobbyScene = uI_LobbyScene;
        GetImage((int)Images.RuneImage).sprite = Managers.Rune.RuneSprites[rune.gradeOfRune];
        GetImage((int)Images.RuneTextImage).sprite = Managers.Rune.RuneTextImages[rune.baseRune];
        GetImage((int)Images.RuneImage).enabled = true;
        GetImage((int)Images.RuneTextImage).enabled = true;
    }
    public void OffImage()
    {
        _rune = null;
        GetImage((int)Images.RuneImage).sprite = null;
        GetImage((int)Images.RuneTextImage).sprite = null;
        GetImage((int)Images.RuneImage).enabled = false;
        GetImage((int)Images.RuneTextImage).enabled = false;
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
