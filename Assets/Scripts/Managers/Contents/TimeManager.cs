using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TimeManager
{
    public bool IsPause { get; private set; } = true;

    public float GameTime { get; private set; } = 0f;
    public float CurStageTime { get; private set; } = 0f;
    private float _curMonsterRespawnTime = 0f;

    public int CurTimeScale { get; private set; } = 1;

    public Action OnNextStage;
    public Action OnMonsterRespawnTime;

    public void Init()
    {
        IsPause = false;

        GameTime = 0f;
        CurStageTime = 0f;
        _curMonsterRespawnTime = 0f;

        CurTimeScale = 1;
        Time.timeScale = CurTimeScale;
    }


    public void OnUpdate()
    {
        if (IsPause)
            return;
        float deltatime = Time.deltaTime;
        GameTime += deltatime;
        CurStageTime += deltatime;
        _curMonsterRespawnTime += deltatime;

        // 데이터로 설정된 스테이지의 정보를 불러와서 지정
        if (Managers.Data.StageDict.TryGetValue(Managers.Game.CurStage, out StageData stageData) == false)
            return;
        if (_curMonsterRespawnTime >= stageData.respawnTime)
        {
            Util.CheckTheEventAndCall(OnMonsterRespawnTime);
            _curMonsterRespawnTime = 0f;
        }

        if (CurStageTime > stageData.stageTime)
        {
            // 현재 게임 스테이지가 isSpecial(ex)보스전) 일 때
            // 몬스터를 전부 잡지 못했다면 게임오버
            if (stageData.isSpecial
                && Managers.Game.Monsters.Count > 0)
            {
                Managers.Game.GameOver("Fail");
            }
            Util.CheckTheEventAndCall(OnNextStage);
            CurStageTime = 0f;
        }
    }

    public void SkipStage()
    {
        if (Managers.Game.Monsters.Count > 0)
            return;
        Util.CheckTheEventAndCall(OnNextStage);
        CurStageTime = 0f;
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
                if (Managers.Data.StageDict.TryGetValue(Managers.Game.CurStage, out StageData stageData) == false)
                    return "FALSE";
                return TimeSpan.FromSeconds(stageData.stageTime - CurStageTime).ToString(@"mm\:ss");
            case StageTimeType.StageTime:
                return TimeSpan.FromSeconds(CurStageTime).ToString(@"mm\ : ss");
            default:
                return TimeSpan.FromSeconds(CurStageTime).ToString(@"mm\ : ss");

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

    public void Clear()
    {
        IsPause = true;
        CurTimeScale = 1;
        Time.timeScale = CurTimeScale;
    }
}
