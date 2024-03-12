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
    }

    public override void Clear()
    {

    }

    public void LoginSceneButtonClicked()
    {
        // 플레이어 데이터 확인(처음인지, 아닌지)
        // 플레이어 데이터 저장 or 로드
        // if(처음일 때)
        Managers.UI.ShowPopupUI<UI_OptionMenu>();
        // else
        // Managers.Scene.LoadScene(Define.Scene.Lobby);
    }
}
