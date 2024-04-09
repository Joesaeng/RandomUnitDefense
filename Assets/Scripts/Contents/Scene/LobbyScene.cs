using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    UI_LobbyScene _ui_Scene;
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Lobby;
        Managers.Game.CurrentScene = Define.Scene.Lobby;
        _ui_Scene = Managers.UI.ShowSceneUI<UI_LobbyScene>();
        _ui_Scene.Scene = this;
        Managers.Sound.Play("LobbyScene", Define.Sound.Bgm);
    }


    public override void Clear()
    {
        _ui_Scene.Clear();
    }
}
