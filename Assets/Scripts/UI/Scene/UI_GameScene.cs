using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene
{
    enum Buttons
    {
        BtnSpawn,
        BtnSlot1,
        BtnSlot2,
        BtnSlot3,
        BtnSlot4,
        BtnSlot5,
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
        UnitImage1,
        UnitImage2,
        UnitImage3,
        UnitImage4,
        UnitImage5,
        FillMonsterGageBar
    }

    public override void Init()
    {
        base.Init();
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
        MonsterData monsterData = Managers.Data.GetMonsterData(Managers.Game.CurStage);
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
