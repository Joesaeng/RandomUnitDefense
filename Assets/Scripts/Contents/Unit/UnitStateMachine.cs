using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Unit;

public enum UnitState
{
    SearchTarget,           // 타겟 스캔
    SkillState,             // 스킬 사용
}

[Serializable]
public class UnitStateMachine //: MonoBehaviour
{
    GameObject _ownObj;
    
    Monster _targetMonster;

    //public BaseUnit _baseUnit;

    //UnitStat_Base _stat;

    //public UnitStat_Base Stat => _stat;

    private UnitNames       _baseUnit;
    private UnitType        _attackType;
    private int             _unitId;
    private int             _unitLv;
    private float           _attackRate;
    private float           _attackRange;

    public  float           AttackRange => _attackRange;
    public  UnitNames       BaseUnit => _baseUnit;

    private UnitState _state;

    [SerializeField]
    float _curAttackRateTime;

    // WaitForSeconds _attackRate;

    public void OnUpdate()
    {
        _curAttackRateTime += Time.deltaTime;

        switch (_state)
        {
            case UnitState.SearchTarget:
                SearchTargetM();
                break;
            case UnitState.SkillState:
                SkillStateM();
                break;

        }
    }

    public void Init(GameObject ownObj, int unitId, int level)
    {
        _ownObj = ownObj;
        _baseUnit = (UnitNames)unitId;
        _unitId = unitId;
        _unitLv = level;

        UnitStatus unitStatus = Managers.UnitStatus.GetUnitStatus(_baseUnit, _unitLv);
        _attackType = unitStatus.unitType;
        _attackRange = unitStatus.attackRange;
        _attackRate = unitStatus.attackRate;

        // _attackRate = new WaitForSeconds(_stat.attackRate);
        ChangeState(UnitState.SearchTarget);
    }

    public void ChangeState(UnitState state)
    {
        //StopCoroutine(_state.ToString());

        _state = state;

        //StartCoroutine(_state.ToString());
    }

    private void SearchTargetM()
    {
        float closestDist = Mathf.Infinity;
        foreach (Monster monster in Managers.Game.Monsters)
        {
            if (monster.IsDead)
                continue;
            float dist = Util.GetDistance(monster,_ownObj);
            if (dist <= _attackRange && dist <= closestDist)
            {
                closestDist = dist;
                _targetMonster = monster;
            }
        }

        if (_targetMonster != null)
        {
            ChangeState(UnitState.SkillState);
            return;
        }
    }

    private void SkillStateM()
    {
        if (_targetMonster == null ||
            (_targetMonster != null && _targetMonster.IsDead))
        {
            _targetMonster = null;
            ChangeState(UnitState.SearchTarget);
            return;
        }

        float dist = Util.GetDistance(_targetMonster,_ownObj);
        if (dist > _attackRange)
        {
            _targetMonster = null;
            ChangeState(UnitState.SearchTarget);
            return;
        }

        if (_curAttackRateTime > _attackRate)
            Skill();
    }
    /*
    private IEnumerator SearchTarget()
    {
        while (true)
        {
            float closestDist = Mathf.Infinity;
            foreach (Monster monster in Managers.Game.Monsters)
            {
                if (monster.IsDead)
                    continue;
                float dist = Util.GetDistance(monster,_ownObj);
                if (dist <= _stat.attackRange && dist <= closestDist)
                {
                    closestDist = dist;
                    _targetMonster = monster;
                }
            }

            if (_targetMonster != null)
            {
                ChangeState(UnitState.SkillState);
                break;
            }

            yield return null;
        }
    }

    private IEnumerator SkillState()
    {
        while (true)
        {
            if (_targetMonster == null ||
                (_targetMonster != null && _targetMonster.IsDead))
            {
                _targetMonster = null;
                ChangeState(UnitState.SearchTarget);
                break;
            }

            float dist = Util.GetDistance(_targetMonster,_ownObj);
            if (dist > _stat.attackRange)
            {
                _targetMonster = null;
                ChangeState(UnitState.SearchTarget);
                break;
            }

            Skill();
            // yield return _attackRate;
        }
    }
    */ // 코루틴 스테이트머신

    private void Skill()
    {
        GameObject bullet = Managers.Game.Spawn("UnitBullet");
        bullet.transform.position = _ownObj.transform.position;
        bullet.GetComponent<UnitBullet>().Init(_targetMonster, _baseUnit,_unitLv);
        /*
        switch (_baseUnit.baseUnit)
        {
            // Common
            case BaseUnits.Knight:
            case BaseUnits.Spearman:
            case BaseUnits.Archer:
            {
                _target.GetComponent<Monster>().TakeHit(Color.black, _stat);
                break;
            }
            // AOE
            case BaseUnits.FireMagician:
            case BaseUnits.Viking:
            case BaseUnits.Warrior:
            {
                float wideAttackRange = 0f;
                if (_stat is AOE stat)
                    wideAttackRange = stat.wideAttackArea;

                HashSet<Monster> monsters = Managers.Game.Monsters;
                foreach (Monster monster in monsters)
                {
                    if (Util.GetDistance(_target, monster.gameObject) <= wideAttackRange)
                    {
                        monster.TakeHit(Color.red, _stat);
                    }
                }
                break;
            }
            // Debuffer
            case BaseUnits.SlowMagician:
            {
                float wideAttackRange = 0f;
                if (_stat is AOE stat)
                    wideAttackRange = stat.wideAttackArea;

                HashSet<Monster> monsters = Managers.Game.Monsters;
                foreach (Monster monster in monsters)
                {
                    if (Util.GetDistance(_target, monster.gameObject) <= wideAttackRange)
                    {
                        monster.TakeHit(Color.green, _stat, Define.AttackType.SlowMagic);
                    }
                }
                break;
            }
            case BaseUnits.StunGun:
            {
                _target.GetComponent<Monster>().TakeHit(Color.yellow, _stat, Define.AttackType.Stun);
                break;
            }
            case BaseUnits.PoisonBowMan:
            {
                _target.GetComponent<Monster>().TakeHit(Color.black, _stat, Define.AttackType.Poison);
                break;
            }
        }*/ // Skill
        _curAttackRateTime = 0f;
    }
}
