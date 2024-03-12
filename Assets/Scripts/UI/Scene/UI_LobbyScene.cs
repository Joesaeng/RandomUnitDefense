using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    enum Texts
    {
        TextTabCombat,
        TextTabBarrack,
        TextTabShop,
        TextStartCombat,
    }

    enum Buttons
    {
        BtnStartCombat
    }
    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetTMPro((int)Texts.TextTabCombat).text = Language.Combat;
        GetTMPro((int)Texts.TextTabBarrack).text = Language.Barrack;
        GetTMPro((int)Texts.TextTabShop).text = Language.Shop;
        GetTMPro((int)Texts.TextStartCombat).text = Language.StartCombat;

        GetButton((int)Buttons.BtnStartCombat).gameObject.AddUIEvent(OnStartCombatButtonClicked);
    }
    public void OnStartCombatButtonClicked(PointerEventData data)
    {

    }
}
