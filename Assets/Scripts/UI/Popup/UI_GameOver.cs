using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameOver : UI_Popup
{
    enum Images
    {
        GameOverImage,
    }

    enum Texts
    {
        TextGameOverStage,
        TextHighestStage,
        TextKillMonsterCount,
        TextEarnedGoldCoin,
        TextLobby,
        TextRetry,
        ValueGameOverStage,
        ValueHighestStage,
        ValueKillMonsterCount,
        ValueEarnedGoldCoin,
    }

    enum Buttons
    {
        BtnLobby,
        BtnRetry
    }
    private void Start()
    {
        
    }
    private void Awake()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
    }
    public void SetUp(string gameoverType)
    {
        // �¸� or �й� �̹��� ����
        GetImage((int)Images.GameOverImage).sprite = Managers.Resource.Load<Sprite>($"Art/UIImages/{gameoverType}");
        GetImage((int)Images.GameOverImage).SetNativeSize();

        // ��� ����
        GetTMPro((int)Texts.TextGameOverStage).text = Language.GameOverStage;
        GetTMPro((int)Texts.TextHighestStage).text = Language.HighestStage;
        GetTMPro((int)Texts.TextKillMonsterCount).text = Language.KillMonsterCount;
        GetTMPro((int)Texts.TextEarnedGoldCoin).text = Language.EarnedGoldCoin;
        GetTMPro((int)Texts.TextLobby).text = Language.Lobby;
        GetTMPro((int)Texts.TextRetry).text = Language.Retry;

        // ��� ����
        GetTMPro((int)Texts.ValueGameOverStage).text = $"{Managers.Game.CurStage}";
        GetTMPro((int)Texts.ValueHighestStage).text = $"{Managers.Player.Data.HighestStage}";
        GetTMPro((int)Texts.ValueKillMonsterCount).text = $"{Managers.Game.KillMonsterCount}";
        GetTMPro((int)Texts.ValueEarnedGoldCoin).text = $"{Managers.Game.EarnedGoldCoin}";

        // ��ư�̺�Ʈ ���ε�
        GetButton((int)Buttons.BtnLobby).gameObject.AddUIEvent(OnButtonLobbyClick);
        GetButton((int)Buttons.BtnRetry).gameObject.AddUIEvent(OnButtonRetryClick);
    }

    public void OnButtonLobbyClick(PointerEventData eventData)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        Managers.Time.GameResume();
        Managers.Scene.LoadSceneWithLoadingScene(Define.Scene.Lobby);
    }

    public void OnButtonRetryClick(PointerEventData eventData)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        Managers.Time.GameResume();
        Managers.Scene.LoadSceneWithLoadingScene(Define.Scene.Combat);
    }
}