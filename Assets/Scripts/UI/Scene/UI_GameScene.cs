using Data;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene
{
    enum GameObjects
    {
        PanelUpgrade,
    }
    enum Buttons
    {
        BtnSpawn,
    }
    enum TMPros
    {
        TextSpawn,
        TextTheAmountOfRuby,
        TextStage,
        TextMonsterInfo,
        TextLeftTimeStage,
        TextChangeMonsterCount,
        TextFixedMonsterCount,
    }

    enum Images
    {
        FillMonsterGageBar
    }

    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TMPros));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.BtnSpawn).gameObject.AddUIEvent(OnSpawnButtonClicked);
        GetTMPro((int)TMPros.TextFixedMonsterCount).text = ConstantData.MonsterCountForGameOver.ToString();

        OnChangeAmountOfRuby(Managers.Game.Ruby);
        Managers.Game.OnChangedRuby -= OnChangeAmountOfRuby;
        Managers.Game.OnChangedRuby += OnChangeAmountOfRuby;

        OnNextStageEvent();
        Managers.Game.OnNextStage -= OnNextStageEvent;
        Managers.Game.OnNextStage += OnNextStageEvent;

        GameObject panelUpgrade = Get<GameObject>((int)GameObjects.PanelUpgrade);
        foreach (Transform child in panelUpgrade.transform)
            Managers.Resource.Destroy(child.gameObject);

        int unitCount = Managers.Game._selectedUnitIds.Length;
        for(int i = 0; i < unitCount; i++)
        {
            GameObject upgradeBtn = Managers.UI.MakeSubItem<UI_BtnUpgrade>(parent : panelUpgrade.transform).gameObject;
            upgradeBtn.transform.localScale = new Vector3(1f, 1f, 1f);
            upgradeBtn.GetComponent<UI_BtnUpgrade>().SetInfo(i, Managers.Game._selectedUnitIds[i]);
        }
    }

    private void Update()
    {
        GetTMPro((int)TMPros.TextLeftTimeStage).text = Managers.Time.GetStageTimeByTimeDisplayFormat(TimeManager.StageTimeType.LeftTime);
        GetTMPro((int)TMPros.TextChangeMonsterCount).text = $"{Managers.Game.Monsters.Count}";
        GetImage((int)Images.FillMonsterGageBar).fillAmount =
            Util.CalculatePercent(Managers.Game.Monsters.Count, ConstantData.MonsterCountForGameOver);
    }

    public void OnNextStageEvent()
    {
        GetTMPro((int)TMPros.TextStage).text = $"STAGE {Managers.Game.CurStage}";
        MonsterData monsterData = Managers.Data.GetMonsterData(1);
        GetTMPro((int)TMPros.TextMonsterInfo).text = 
            $"<sprite=46> : {monsterData.maxHp}\n" +
            $"<sprite=50> : {monsterData.defense}";

    }

    public void OnSpawnButtonClicked(PointerEventData data)
    {
        Managers.Game.OnSpawnButtonClicked();
    }

    public void OnChangeAmountOfRuby(int value)
    {
        GetTMPro((int)TMPros.TextTheAmountOfRuby).text = $"<sprite=25> {value}";
    }
}
