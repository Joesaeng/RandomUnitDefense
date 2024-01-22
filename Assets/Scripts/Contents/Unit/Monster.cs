using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public enum DebuffState
    {
        None,
        Slow,
        Poison,
        Stun,
        Count,
    }
    
    float[] _debuffTimes = new float[(int)DebuffState.Count];
    float[] _debuffRatios = new float[(int)DebuffState.Count];

    Vector3[] _movePoints;
    int _nextMovePoint;

    bool isStun = false;

    [SerializeField]
    float _moveSpeed;

    float _curMoveSpeed;

    int _maxHp = 10;
    [SerializeField]
    float _curHp;

    public void Init(int stageNum, Define.Map map)
    {
        SetMovePoint(map);
        _nextMovePoint = 0;

        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        tColor = new Color(1f, 56 / 255f, 0f);
        spr.color = tColor;

        _curHp = _maxHp;
        // stageNum에 따라서 유닛의 형태, 이동속도, 체력 등 초기화
    }

    private void SetMovePoint(Define.Map map)
    {
        switch (map)
        {
            case Define.Map.Basic:
            {
                _movePoints = ConstantData.BasicMapPoint;
            }
            break;
        }
    }

    public void MonsterUpdate()
    {
        DeBuffUpdate();
        Move();
    }
    private void DeBuffUpdate()
    {
        for(int i = (int)DebuffState.None; i < (int)DebuffState.Count; ++i)
        {
            if (_debuffTimes[i] > -0.01f)
                _debuffTimes[i] -= Time.deltaTime;
            Debuff((DebuffState)i, _debuffTimes[i] > 0f);
        }
    }
    private void Debuff(DebuffState debuffState, bool onOff)
    {
        if(onOff == true)
        {
            switch (debuffState)
            {
                case DebuffState.Slow:
                    if(_curMoveSpeed >= _moveSpeed * _debuffRatios[(int)DebuffState.Slow])
                        _curMoveSpeed = _moveSpeed * _debuffRatios[(int)DebuffState.Slow];
                    break;
                case DebuffState.Poison:
                    _curHp -= _debuffRatios[(int)(DebuffState.Poison)] * Time.deltaTime;
                    break;
                case DebuffState.Stun:
                    isStun = true;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (debuffState)
            {
                case DebuffState.Slow:
                    _curMoveSpeed = _moveSpeed;
                    break;
                case DebuffState.Poison:
                    break;
                case DebuffState.Stun:
                    isStun = false;
                    break;
                default:
                    break;
            }
        }
        
    }    

    private void Move()
    {
        if (isStun)
            return;
        if (Vector3.Distance(transform.position, _movePoints[_nextMovePoint]) <= 0.01f)
        {
            _nextMovePoint++;
            _nextMovePoint %= _movePoints.Length;
        }
        transform.position = Vector3.MoveTowards(transform.position, _movePoints[_nextMovePoint], _curMoveSpeed * Time.deltaTime);
    }

    Color tColor;
    public void TakeHit(Color color, UnitStat_Base stat, Define.AttackType attackType = Define.AttackType.Common)
    {
        SpriteRenderer spr = GetComponent<SpriteRenderer>();

        spr.color = color;

        // 히트 이펙트 실행(Image, Sound 등)

        switch (attackType)
        {
            case Define.AttackType.SlowMagic:
            {
                if (stat is SlowMagician Stat)
                {
                    _debuffTimes[(int)DebuffState.Slow] = Stat.slowDuration;
                    _debuffRatios[(int)DebuffState.Slow] = Stat.slowRatio;
                }
                break;
            }
            case Define.AttackType.Stun:
            {
                if (stat is StunGun Stat)
                {
                    _debuffTimes[(int)DebuffState.Stun] = Stat.stunDuration;
                }
                break;
            }
            case Define.AttackType.Poison:
            {
                if (stat is PoisonBowMan Stat)
                {
                    _debuffTimes[(int)DebuffState.Poison] = Stat.poisonDuration;
                    _debuffRatios[(int)DebuffState.Poison] = Stat.poisonDamagePerSecond;
                }
                break;
            }
        }

        _curHp -= stat.attackDamage;
        if (_curHp <= 0)
        {
            Managers.Game.RegisterDyingMonster(this.gameObject);
            StopAllCoroutines();
        }
        else
        {
            // TEMP
            StartCoroutine("CoColor");
        }
    }

    IEnumerator CoColor()
    {
        yield return new WaitForSeconds(0.1f);
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        spr.color = tColor;
    }
}
