using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Lobby;
        Managers.Game.CurrentScene = Define.Scene.Lobby;
        //UI_LobbyScene ui_lobbyScene = Managers.UI.ShowSceneUI<UI_LobbyScene>();
        //ui_lobbyScene.Scene = this;
    }


    public override void Clear()
    {

    }
}
