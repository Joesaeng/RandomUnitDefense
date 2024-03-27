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
        // 승리 or 패배 이미지 세팅
        GetImage((int)Images.GameOverImage).sprite = Managers.Resource.Load<Sprite>($"Art/UIImages/{gameoverType}");
        GetImage((int)Images.GameOverImage).SetNativeSize();

        // 언어 세팅
        GetText((int)Texts.TextGameOverStage).text = Language.GameOverStage;
        GetText((int)Texts.TextHighestStage).text = Language.HighestStage;
        GetText((int)Texts.TextKillMonsterCount).text = Language.KillMonsterCount;
        GetText((int)Texts.TextEarnedGoldCoin).text = Language.EarnedGoldCoin;
        GetText((int)Texts.TextLobby).text = Language.Lobby;
        GetText((int)Texts.TextRetry).text = Language.Retry;

        // 밸류 세팅
        GetText((int)Texts.ValueGameOverStage).text = $"{Managers.Game.CurStage}";
        GetText((int)Texts.ValueHighestStage).text = $"{Managers.Player.Data.highestStage}";
        GetText((int)Texts.ValueKillMonsterCount).text = $"{Managers.Game.KillMonsterCount}";
        GetText((int)Texts.ValueEarnedGoldCoin).text = $"{Managers.Game.EarnedGoldCoin}";

        // 버튼이벤트 바인딩
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
