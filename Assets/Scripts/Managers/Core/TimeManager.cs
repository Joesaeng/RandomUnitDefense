using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TimeManager
{
    public bool IsPause { get; private set; } = true;

    public float GameTime { get; private set; } = 0f;
    public float StageTime { get; private set; } = 0f;
    private float _curMonsterRespawnTime = 0f;

    public Action OnNextStage;
    public Action OnMonsterRespawnTime;

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            SetPause(!IsPause);

        if(IsPause) return;

        GameTime += Time.deltaTime;
        StageTime += Time.deltaTime;
        _curMonsterRespawnTime += Time.deltaTime;

        if(_curMonsterRespawnTime >= ConstantData.MonsterRespawnTime)
        {
            if (OnMonsterRespawnTime != null)
                OnMonsterRespawnTime.Invoke();
            _curMonsterRespawnTime = 0f;
        }

        if (StageTime > ConstantData.OneStageTime)
        {
            StageTime = 0f;
            if (OnNextStage != null)
                OnNextStage.Invoke();
        }
    }

    public string GetStageTimeByTimeDisplayFormat()
    {
        return TimeSpan.FromSeconds(StageTime).ToString(@"mm\:ss");
    }

    public string GetGameTimeByTimeDisplayFormat()
    {
        return TimeSpan.FromSeconds(GameTime).ToString(@"mm\:ss");
    }

    public void SetPause(bool value)
    {
        IsPause = value;
    }
}