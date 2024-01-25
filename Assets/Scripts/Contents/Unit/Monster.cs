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

    MonsterData _monsterStat = new MonsterData();

    Vector3[] _movePoints;
    int _nextMovePoint;

    bool isStun = false;
    bool isDead = false;

    // Stats
    float _moveSpeed;
    float _curMoveSpeed;
    float _curHp;
    int _defense;

    public void Init(int stageNum, Define.Map map)
    {
        SetMovePoint(map);
        _nextMovePoint = 0;
        _monsterStat = Managers.Data.GetMonsterData(stageNum);
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        tColor = new Color(1f, 56 / 255f, 0f);
        spr.color = tColor;

        // stageNum에 따라서 유닛의 형태, 이동속도, 체력 등 초기화
        _curHp = _monsterStat.maxHp;
        _moveSpeed = _monsterStat.moveSpeed;
        _defense = _monsterStat.defense;

        isStun = false;
        isDead = false;
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
        if (isStun || isDead)
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
        if (isDead)
            return;
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

        // 유닛의 공격력이 몬스터의 방어력보다 낮은 경우 1의 데미지를 받게 합니다.
        int damage = stat.attackDamage - _defense > 1 ? stat.attackDamage - _defense : 1;
        _curHp -= damage;
        if (_curHp <= 0)
        {
            isDead = true;
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
