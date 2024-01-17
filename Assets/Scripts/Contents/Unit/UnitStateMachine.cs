using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit;

public class UnitStateMachine
{
    GameObject _ownObj;
    GameObject _target;

    Unit _unitComponent;

    BaseUnit _baseUnit;
    UnitStat_Base _stat;

    float _curSkillCoolTime;

    public void OnUpdate()
    {
        _curSkillCoolTime += Time.deltaTime;
    }

    public UnitStateMachine(GameObject ownObj, int id, int level)
    {
        _ownObj = ownObj;
        _unitComponent = _ownObj.GetComponent<Unit>();
        _baseUnit = Managers.Data.BaseUnitDict[id];
        _stat = Managers.Data.GetUnitData(_baseUnit.baseUnit, level);
    }

    public void TargetScan()
    {
        switch (_baseUnit.type)
        {
            case UnitType.Buffer:
                break;
            default:
                foreach (GameObject obj in Managers.Game.Monsters)
                {
                    if (Util.GetDistance(obj, _ownObj) <= _stat.attackRange)
                    {
                        if (_target == null)
                            _target = obj;
                        else if (Util.GetDistance(_target.gameObject, _ownObj)
                            > Util.GetDistance(obj, _ownObj))
                        {
                            _target = obj;
                        }
                    }
                }
                break;
        }
    }

    public void Chase()
    {
        switch (_baseUnit.type)
        {
            case UnitType.Buffer:
                break;
            default:
                if (_target == null ||
                    (_target != null && Util.GetDistance(_target.gameObject, _ownObj) > _stat.attackRange))
                {
                    TargetScan();
                }
                if (_target != null && _curSkillCoolTime > _stat.attackRate)
                {
                    _unitComponent.State = Unit.UnitState.Skill;
                }
                break;
        }
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
                    wideAttackRange = stat.wideAttackRange;

                HashSet<GameObject> monsters = Managers.Game.Monsters;
                foreach (GameObject obj in monsters)
                {
                    if (Util.GetDistance(_target, obj) <= wideAttackRange)
                    {
                        obj.GetComponent<Monster>().TakeHit(Color.red, _stat);
                    }
                }
                break;
            }
            // Debuffer
            case BaseUnits.SlowMagician:
            {
                float wideAttackRange = 0f;
                if (_stat is AOE stat)
                    wideAttackRange = stat.wideAttackRange;

                HashSet<GameObject> monsters = Managers.Game.Monsters;
                foreach (GameObject obj in monsters)
                {
                    if (Util.GetDistance(_target, obj) <= wideAttackRange)
                    {
                        obj.GetComponent<Monster>().TakeHit(Color.green, _stat,Define.AttackType.SlowMagic);
                    }
                }
                break;
            }
            case BaseUnits.StunGun:
            {
                _target.GetComponent<Monster>().TakeHit(Color.black, _stat,Define.AttackType.Stun);
                break;
            }
            case BaseUnits.PoisonBowMan:
            {
                _target.GetComponent<Monster>().TakeHit(Color.black, _stat,Define.AttackType.Poison);
                break;
            }
            // Buffer
            case BaseUnits.Priest:
            {
                break;
            }
        }
        _curSkillCoolTime = 0f;
        _unitComponent.State = UnitState.Chase;
    }
}
