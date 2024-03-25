using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipedRuneCombatScene : UI_Base
{
    public override void Init()
    {
    }

    public void SetUp(int equipRuneIndex)
    {
        Data.Rune rune = Managers.Player.Data.EquipedRunes[equipRuneIndex];
        GetComponent<Image>().sprite = Managers.Rune.RuneSprites[rune.gradeOfRune];
        Util.FindChild<Image>(gameObject, "TextImage").sprite = Managers.Rune.RuneTextImages[rune.baseRune];
    }

    public override void OnChangeLanguage()
    {
    }
}
