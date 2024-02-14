using Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UnitBullet : MonoBehaviour
{
    private float _bulletSpeed;
    private Monster _targetMonster;
    private Vector3 _targetPosition;

    private UnitStatus _ownUnitStatus;
    private float wideAttackArea;

    public void Init(Monster targetMonster, UnitNames baseUnit, int unitLv, float bulletSpeed = 20f)
    {
        _targetMonster = targetMonster;
        if(targetMonster == null)
        {
            DestroyBullet();
            return;
        }
        _targetPosition = _targetMonster.transform.position;

        _bulletSpeed = bulletSpeed;

        _ownUnitStatus = Managers.UnitStatus.GetUnitStatus(baseUnit, unitLv);
        wideAttackArea = _ownUnitStatus.wideAttackArea;
    }

    void Update()
    {
        if (wideAttackArea > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _bulletSpeed * Time.deltaTime);
            if (Vector3.Distance(gameObject.transform.position, _targetPosition) < 0.1f)
            {
                foreach (Monster monster in Managers.Game.Monsters)
                {
                    if (Vector3.Distance(_targetPosition, monster.transform.position) <= wideAttackArea)
                    {
                        monster.TakeHit(_ownUnitStatus);
                    }
                }

                DestroyBullet();
            }
        }
        else
        {
            if (_targetMonster == null || _targetMonster.IsDead || _targetMonster.gameObject.activeSelf == false)
            {
                // 오브젝트 파괴
                DestroyBullet();
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, _targetMonster.transform.position, _bulletSpeed * Time.deltaTime);
            if (Util.GetDistance(gameObject, _targetMonster.gameObject) < 0.3f)
            {
                _targetMonster.TakeHit(_ownUnitStatus);
                DestroyBullet();
            }
        }
    }

    void DestroyBullet()
    {
        Managers.Resource.Destroy(gameObject);
    }
}
