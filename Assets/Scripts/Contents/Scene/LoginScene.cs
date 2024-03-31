using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        Managers.Init();
        SceneType = Define.Scene.Login;
        Managers.Game.CurrentScene = SceneType;
        Application.targetFrameRate = 60; // ������ ����
    }

    public override void Clear()
    {

    }

    public void LoginSceneButtonClicked()
    {
        Managers.Game.GameLanguage = (Define.GameLanguage)Managers.Player.Data.gameLanguage;
        if (Managers.Player.Data.beginner)
        {
            Managers.UI.ShowPopupUI<UI_OptionMenu>();
            Managers.Player.Data.beginner = false;
        }
        else
            Managers.Scene.LoadSceneWithLoadingScene(Define.Scene.Lobby);

        Managers.Player.SaveToJson();
    }
}
