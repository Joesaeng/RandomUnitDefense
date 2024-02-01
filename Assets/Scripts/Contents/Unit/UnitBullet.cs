using Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UnitBullet : MonoBehaviour
{
    private float _bulletSpeed;
    private Monster _targetMonster;

    private UnitStatus _ownUnitStatus;

    public void Init(Monster targetMonster, UnitNames baseUnit,int unitLv, float bulletSpeed = 20f)
    {
        _targetMonster = targetMonster;
        _bulletSpeed = bulletSpeed;

        _ownUnitStatus = Managers.UnitStatus.GetUnitStatus(baseUnit, unitLv);
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
            float wideAttackArea = _ownUnitStatus.wideAttackArea;
            _targetMonster.TakeHit(_ownUnitStatus);
            if (wideAttackArea > 0)
            {
                foreach (Monster monster in Managers.Game.Monsters)
                {
                    if (_targetMonster == monster)
                        continue;
                    if (Util.GetDistance(_targetMonster, monster) <= wideAttackArea)
                    {
                        monster.TakeHit(_ownUnitStatus);
                    }
                }
            }
            
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        Managers.Resource.Destroy(gameObject);
    }
}
