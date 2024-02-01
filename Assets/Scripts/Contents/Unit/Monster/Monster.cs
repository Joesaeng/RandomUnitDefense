using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster : MonoBehaviour
{
    MonsterData _monsterStat = new MonsterData();

    Vector3[] _movePoints;
    int _nextMovePoint;

    [SerializeField]
    private bool _isStun { get; set; } = false;
    [SerializeField]
    private bool _isDead = false;
    public bool IsDead => _isDead;

    [SerializeField]
    List<BaseDebuff> _debuffs;
    public List<BaseDebuff> Debuffs => _debuffs;

    // Stats
    float _moveSpeed;
    float _curMoveSpeed;
    float _curHp;
    public float CurHp => _curHp;
    public float MaxHp { get; private set; }
    int _defense;

    public void Init(int stageNum, Define.Map map)
    {
        SetMovePoint(map);
        _nextMovePoint = 0;
        _monsterStat = Managers.Data.GetMonsterData(stageNum);

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);

        // stageNum에 따라서 유닛의 형태, 이동속도, 체력 등 초기화
        MaxHp = _monsterStat.maxHp;
        _curHp = _monsterStat.maxHp;
        _moveSpeed = _monsterStat.moveSpeed;
        _curMoveSpeed = _moveSpeed;
        _defense = _monsterStat.defense;

        _debuffs = new List<BaseDebuff>();

        _debuffs.Clear();

        _isDead = false;
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
        for (int i = 0; i < Debuffs.Count; ++i)
        {
            Debuffs[i].OnUpdate();
        }

        Move();
    }

    #region 디버프

    public void ApplySlowDebuff(float ratio)
    {
        _curMoveSpeed = _moveSpeed * ratio;
    }

    public void QuitSlowDebuff()
    {
        _curMoveSpeed = _moveSpeed;
    }

    public void ApplyStunDebuff()
    {
        _isStun = true;
    }

    public void QuitStunDebuff()
    {
        _isStun = false;
    }

    private void ApplyDebuff(BaseDebuff debuff)
    {
        _debuffs.Add(debuff);
    }

    private void QuitAllDebuff()
    {
        _isStun = false;
        for (int i = 0; i < Debuffs.Count; ++i)
        {
            QuitDebuff(Debuffs[i]);
        }
    }

    public void QuitDebuff(BaseDebuff debuff)
    {
        _debuffs.Remove(debuff);
    }

    #endregion

    private void Move()
    {
        if (IsDead || _isStun)
            return;
        if (Vector3.Distance(transform.position, _movePoints[_nextMovePoint]) <= 0.01f)
        {
            _nextMovePoint++;
            _nextMovePoint %= _movePoints.Length;
        }
        transform.position = Vector3.MoveTowards(transform.position, _movePoints[_nextMovePoint], _curMoveSpeed * Time.deltaTime);
    }

    public void TakeHit(UnitStatus attackerStat)
    {
        if (IsDead)
            return;
        UnitNames unit = attackerStat.unit;

        // 히트 이펙트 실행(Image, Sound 등)

        switch (unit)
        {
            case UnitNames.SlowMagician:
            {
                SlowDebuff slowDebuff = new SlowDebuff();
                slowDebuff.Init(this, attackerStat.debuffDuration, attackerStat.debuffRatio);
                ApplyDebuff(slowDebuff);
                break;
            }
            case UnitNames.StunGun:
            {
                StunDebuff stunDebuff = new StunDebuff();
                stunDebuff.Init(this, attackerStat.debuffDuration);
                ApplyDebuff(stunDebuff);
                break;
            }
            case UnitNames.PoisonBowMan:
            {
                PoisonDebuff poisonDebuff = new PoisonDebuff();
                poisonDebuff.Init(this, attackerStat.debuffDuration, attackerStat.damagePerSecond);
                ApplyDebuff(poisonDebuff);
                break;
            }
        }

        // 유닛의 공격력이 몬스터의 방어력보다 낮은 경우 1의 데미지를 받게 합니다.
        float damage = attackerStat.attackDamage - _defense > 1 ? attackerStat.attackDamage - _defense : 1;
        ReduceHp(damage);
    }

    public void ReduceHp(float damage)
    {
        _curHp -= damage;
        if (_curHp <= 0)
        {
            _isDead = true;
            Managers.Game.RegisterDyingMonster(this.gameObject);
            QuitAllDebuff();
            StopAllCoroutines();
        }
    }
}
