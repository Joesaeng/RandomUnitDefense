using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RuneSlot : UI_LobbySceneSlot
{
    UI_LobbyScene _uI_LobbyScene;

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
        base.Init();
        parentSlotsScroll = GameObject.FindWithTag("RuneSlotsScroll").GetComponent<ScrollRect>();
    }
}
