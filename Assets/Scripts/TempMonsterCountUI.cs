using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TempMonsterCountUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI tttt;
    [SerializeField]
    TextMeshProUGUI tttt2;
    void Update()
    {
        tttt.text = $"���� ī��Ʈ : {Managers.Game.Monsters.Count}";
        tttt2.text = $"�������� {Managers.Game.CurStage}  " +
            $"| �ð� : {Managers.Time.GetStageTimeByTimeDisplayFormat()}";
    }
}
