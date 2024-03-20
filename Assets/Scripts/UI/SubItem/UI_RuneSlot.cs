using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_RuneSlot : UI_LobbySceneSlot
{
    UI_LobbyScene _ui_LobbyScene;
    Rune _rune;
    public int _runeIndex;
    enum Images
    {
        RuneImage,
        EquipedRuneImage
    }    

    enum Texts
    {
        RuneText
    }

    public override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));
        parentSlotsScroll = GameObject.FindWithTag("RuneSlotsScroll").GetComponent<ScrollRect>();

        gameObject.AddUIEvent(ClickedRuneSlot);
    }

    public void SetRune(int runeIndex,UI_LobbyScene uI_LobbyScene)
    {
        _runeIndex = runeIndex;
        _ui_LobbyScene = uI_LobbyScene;
        _rune = Managers.Player.Data.ownedRunes[_runeIndex];

        GetImage((int)Images.RuneImage).sprite = Managers.Rune.RuneSprites[_rune.gradeOfRune];
        GetImage((int)Images.EquipedRuneImage).enabled = _rune.isEquip;
        GetTMPro((int)Texts.RuneText).text = Managers.Rune.RuneTextImages[_rune.baseRune];
    }

    public void ClickedRuneSlot(PointerEventData data)
    {
        if (!forParent)
        {
            Managers.Sound.Play(Define.SFXNames.Click);
            _ui_LobbyScene.UpdateSelectedRunePanel(_rune,_runeIndex);
        }
    }

}
