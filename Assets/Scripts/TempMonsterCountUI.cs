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
        tttt.text = $"몬스터 카운트 : {Managers.Game.Monsters.Count}";
        tttt2.text = $"스테이지 {Managers.Game.CurStage}  " +
            $"| 시간 : {Managers.Time.GetStageTimeByTimeDisplayFormat()}";
    }
}
