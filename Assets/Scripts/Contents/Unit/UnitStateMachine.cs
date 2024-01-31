using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Unit;

public enum UnitState
{
    SearchTarget,       // Å¸°Ù ½ºÄµ
    SkillState,              // ½ºÅ³ »ç¿ë
}

[Serializable]
public class UnitStateMachine : MonoBehaviour
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

    private UnitState _state;

    [SerializeField]
    float _curAttackRateTime;

    WaitForSeconds _attackRate;

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

    public void Init(GameObject ownObj, int id, int level)
    {
        _ownObj = ownObj;
        _baseUnit = Managers.Data.BaseUnitDict[id];
        _stat = Managers.Data.GetUnitData(_baseUnit.baseUnit, level);
        _attackRate = new WaitForSeconds(_stat.attackRate);
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
            float dist = Vector3.Distance(monster.transform.position,_ownObj.transform.position);
            if (dist <= _stat.attackRange && dist <= closestDist)
            {
                closestDist = dist;
                _target = monster.gameObject;
            }
        }

        if (_target != null)
        {
            ChangeState(UnitState.SkillState);
            return;
        }
    }

    private void SkillStateM()
    {
        if (_target == null ||
            (_target != null && _target.GetComponent<Monster>().IsDead))
        {
            _target = null;
            ChangeState(UnitState.SearchTarget);
            return;
        }

        float dist = Util.GetDistance(_target,_ownObj);
        if (dist > _stat.attackRange)
        {
            _target = null;
            ChangeState(UnitState.SearchTarget);
            return;
        }

        if (_curAttackRateTime > _stat.attackRate)
            Skill();
    }

    private IEnumerator SearchTarget()
    {
        while (true)
        {
            float closestDist = Mathf.Infinity;
            foreach (Monster monster in Managers.Game.Monsters)
            {
                if (monster.IsDead)
                    continue;
                float dist = Vector3.Distance(monster.transform.position,_ownObj.transform.position);
                if (dist <= _stat.attackRange && dist <= closestDist)
                {
                    closestDist = dist;
                    _target = monster.gameObject;
                }
            }

            if (_target != null)
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
            if (_target == null ||
                (_target != null && _target.GetComponent<Monster>().IsDead))
            {
                _target = null;
                ChangeState(UnitState.SearchTarget);
                break;
            }

            float dist = Util.GetDistance(_target,_ownObj);
            if (dist > _stat.attackRange)
            {
                _target = null;
                ChangeState(UnitState.SearchTarget);
                break;
            }

            Skill();
            yield return _attackRate;
        }
    }

    private void Skill()
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
        }
        _curAttackRateTime = 0f;
    }
}
