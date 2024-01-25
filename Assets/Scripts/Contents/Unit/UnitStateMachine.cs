using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit;

[Serializable]
public class UnitStateMachine
{
    GameObject _ownObj;
    [SerializeField]
    GameObject _target;

    BaseUnit _baseUnit;
    public BaseUnits GetBaseUnit
    {
        get { return _baseUnit.baseUnit; }
    }
    UnitStat_Base _stat;

    public UnitStat_Base Stat => _stat;

    private UnitState _state = UnitState.Idle;
    public UnitState State { get { return _state; } set { _state = value; } }

    float _curSkillCoolTime;

    public void OnUpdate()
    {
        _curSkillCoolTime += Time.deltaTime;
        switch (State)
        {
            case UnitState.Idle:
                //Idle();
                break;
            case UnitState.Chase:
                Chase();
                break;
            case UnitState.Skill:
                Skill();
                break;
        }
    }

    public UnitStateMachine(GameObject ownObj, int id, int level)
    {
        _ownObj = ownObj;
        _baseUnit = Managers.Data.BaseUnitDict[id];
        _stat = Managers.Data.GetUnitData(_baseUnit.baseUnit, level);
        State = UnitState.Chase;
    }

    public void TargetScan()
    {
        foreach (Monster monster in Managers.Game.Monsters)
        {
            if (Util.GetDistance(monster.gameObject, _ownObj) <= _stat.attackRange)
            {
                if (_target == null)
                    _target = monster.gameObject;
                else if (Util.GetDistance(_target.gameObject, _ownObj)
                    > Util.GetDistance(monster.gameObject, _ownObj))
                {
                    _target = monster.gameObject;
                }
            }
        }
    }

    public void Chase()
    {
        if (NeedToTargetScan())
        {
            TargetScan();
        }
        if (_target != null && _curSkillCoolTime > _stat.attackRate)
        {
            State = Unit.UnitState.Skill;
        }
    }

    private bool NeedToTargetScan()
    {
        // 타겟이 null 이거나, 타겟이 unactive(pool에 들어감) 이거나,
        // 타겟이 공격 범위를 벗어났거나
        bool ret = (_target == null || _target.gameObject.activeSelf == false ||
            ((_target != null && _target.gameObject.activeSelf == true)
            && Util.GetDistance(_target.gameObject, _ownObj) > _stat.attackRange));
        return ret;
    }

    public void Skill()
    {
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
                        monster.gameObject.GetComponent<Monster>().TakeHit(Color.red, _stat);
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
                        monster.gameObject.GetComponent<Monster>().TakeHit(Color.green, _stat, Define.AttackType.SlowMagic);
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
        }
        _curSkillCoolTime = 0f;
        State = Unit.UnitState.Chase;
    }
}
