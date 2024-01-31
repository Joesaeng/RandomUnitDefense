using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBullet : MonoBehaviour
{
    private float _bulletSpeed;
    private Monster _targetMonster;

    private UnitStat_Base _stat;
    private BaseUnits _baseUnit;
    private UnitType _unitType;

    public void Init(Monster targetMonster, UnitStat_Base unitStat,BaseUnits baseUnit,UnitType unitType, float bulletSpeed = 10f)
    {
        _targetMonster = targetMonster;
        _stat = unitStat;
        _baseUnit = baseUnit;
        _unitType = unitType;
        _bulletSpeed = bulletSpeed;
    }

    void Update()
    {
        if(_targetMonster == null || _targetMonster.IsDead || _targetMonster.gameObject.activeSelf == false)
        {
            // 오브젝트 파괴
            DestroyBullet();
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position,_targetMonster.transform.position, _bulletSpeed * Time.deltaTime);
        if(Util.GetDistance(gameObject,_targetMonster.gameObject) < 0.3f)
        {
            switch (_unitType)
            {
                case UnitType.Common:
                    _targetMonster.TakeHit(Color.black, _stat);
                    
                    // 오브젝트 파괴
                    break;
                case UnitType.AOE:
                {
                    float wideAttackRange = 0f;
                    if (_stat is AOE stat)
                        wideAttackRange = stat.wideAttackArea;

                    foreach (Monster monster in Managers.Game.Monsters)
                    {
                        if (Util.GetDistance(_targetMonster, monster) <= wideAttackRange)
                        {
                            monster.TakeHit(Color.red, _stat);
                        }
                    }
                    break;
                }
                case UnitType.Debuffer:
                {
                    switch (_baseUnit)
                    {
                        case BaseUnits.SlowMagician:
                        {
                            float wideAttackRange = 0f;
                            if (_stat is AOE stat)
                                wideAttackRange = stat.wideAttackArea;

                            foreach (Monster monster in Managers.Game.Monsters)
                            {
                                if (Util.GetDistance(_targetMonster, monster) <= wideAttackRange)
                                {
                                    monster.TakeHit(Color.green, _stat, Define.AttackType.SlowMagic);
                                }
                            }
                            break;
                        }
                        case BaseUnits.StunGun:
                        {
                            _targetMonster.TakeHit(Color.yellow, _stat, Define.AttackType.Stun);
                            break;
                        }
                        case BaseUnits.PoisonBowMan:
                        {
                            _targetMonster.TakeHit(Color.black, _stat, Define.AttackType.Poison);
                            break;
                        }
                    }
                }
                    break;
            }
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        Managers.Resource.Destroy(gameObject);
    }
}
