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
        RuneTextImage,
        EquipedRuneImage
    }    

    public override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Images));

        gameObject.AddUIEvent(ClickedRuneSlot);
    }

    public void SetRune(int runeIndex,UI_LobbyScene uI_LobbyScene)
    {
        _runeIndex = runeIndex;
        _ui_LobbyScene = uI_LobbyScene;
        _rune = Managers.Player.Data.ownedRunes[_runeIndex];

        GetImage((int)Images.RuneImage).sprite = Managers.Rune.RuneSprites[_rune.gradeOfRune];
        GetImage((int)Images.RuneTextImage).sprite = Managers.Rune.RuneTextImages[_rune.baseRune];
        GetImage((int)Images.EquipedRuneImage).enabled = _rune.isEquip;
    }

    public void ClickedRuneSlot(PointerEventData data)
    {
        if (!forParent)
        {
            Managers.Sound.Play(Define.SFXNames.Click);
            _ui_LobbyScene.UpdateSelectedRunePanel(_rune,_runeIndex);
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        forParent = Mathf.Abs(eventData.delta.x) > 1f && Mathf.Abs(eventData.delta.y) > 1f;

        if (forParent)
        {
            NM.OnBeginDrag(eventData);
            baseScrollRect.OnBeginDrag(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            NM.OnDrag(eventData);
            baseScrollRect.OnDrag(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            NM.OnEndDrag(eventData);
            baseScrollRect.OnEndDrag(eventData);
        }
    }
}
