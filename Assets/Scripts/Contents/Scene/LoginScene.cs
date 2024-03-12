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
        // �÷��̾� ������ Ȯ��(ó������, �ƴ���)
        // �÷��̾� ������ ���� or �ε�
        // if(ó���� ��)
        Managers.UI.ShowPopupUI<UI_OptionMenu>();
        // else
        // Managers.Scene.LoadScene(Define.Scene.Lobby);
    }
}
