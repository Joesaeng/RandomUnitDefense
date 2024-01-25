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
    }

    enum Images
    {
        UnitImage1,
        UnitImage2,
        UnitImage3,
        UnitImage4,
        UnitImage5,
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TMPros));
        Bind<Image>(typeof(Images));

        GetButton((int)Buttons.BtnSpawn).gameObject.AddUIEvent(OnSpawnButtonClicked);

        OnChangeAmountOfRuby(Managers.Game.Ruby);
        Managers.Game.OnChangedRuby -= OnChangeAmountOfRuby;
        Managers.Game.OnChangedRuby += OnChangeAmountOfRuby;
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
