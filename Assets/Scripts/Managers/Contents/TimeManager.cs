using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TimeManager
{
    public bool IsPause { get; private set; } = false;

    public float GameTime { get; private set; } = 0f;
    public float StageTime { get; private set; } = 0f;
    private float _curMonsterRespawnTime = 0f;

    public int CurTimeScale { get; private set; } = 1;

    public Action OnNextStage;
    public Action OnMonsterRespawnTime;

    public void Init()
    {
        IsPause = false;

        GameTime = 0f;
        StageTime = 0f;
        _curMonsterRespawnTime = 0f;

        CurTimeScale = 1;
    }


    public void OnUpdate()
    {
        if (IsPause)
            return;
        float deltatime = Time.deltaTime;
        GameTime += deltatime;
        StageTime += deltatime;
        _curMonsterRespawnTime += deltatime;

        if (_curMonsterRespawnTime >= ConstantData.MonsterRespawnTime)
        {
            Util.CheckTheEventAndCall(OnMonsterRespawnTime);
            _curMonsterRespawnTime = 0f;
        }

        if (StageTime > ConstantData.OneStageTime)
        {
            StageTime = 0f;
            Util.CheckTheEventAndCall(OnNextStage);
        }
    }

    public enum StageTimeType
    {
        LeftTime,
        StageTime
    }
    public string GetStageTimeByTimeDisplayFormat(StageTimeType type)
    {
        switch (type)
        {
            case StageTimeType.LeftTime:
                return TimeSpan.FromSeconds(ConstantData.OneStageTime - StageTime).ToString(@"mm\:ss");
            case StageTimeType.StageTime:
                return TimeSpan.FromSeconds(StageTime).ToString(@"mm\ : ss");
            default:
                return TimeSpan.FromSeconds(StageTime).ToString(@"mm\ : ss");

        }
    }

    public string GetGameTimeByTimeDisplayFormat()
    {
        return TimeSpan.FromSeconds(GameTime).ToString(@"mm\:ss");
    }

    public void GamePause()
    {
        Time.timeScale = 0f;
        IsPause = true;
    }

    public void GameResume()
    {
        Time.timeScale = CurTimeScale;
        IsPause = false;
    }

    public int ChangeTimeScale()
    {
        switch (CurTimeScale)
        {
            case 1:
                Time.timeScale = 2f;
                CurTimeScale = 2;
                break;
            case 2:
                Time.timeScale = 3f;
                CurTimeScale = 3;
                break;
            case 3:
                Time.timeScale = 1f;
                CurTimeScale = 1;
                break;
        }
        return CurTimeScale;
    }
}
