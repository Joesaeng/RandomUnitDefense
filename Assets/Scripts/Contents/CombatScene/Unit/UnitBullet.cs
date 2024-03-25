using Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UnitBullet : MonoBehaviour
{
    private float _bulletSpeed;
    private Monster _targetMonster;
    private Vector3 _targetPosition;

    private UnitStatus _ownUnitStatus;
    private float wideAttackArea;

    SpriteRenderer _spriteRenderer;
    Sprite[] _sprites = new Sprite[ConstantData.PlayerUnitHighestLevel];

    bool isCritical = false;

    public void Init(Monster targetMonster, UnitNames baseUnit, int unitLv, float bulletSpeed = 25f)
    {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_sprites[unitLv - 1] == null)
            _sprites[unitLv - 1] = Managers.Resource.Load<Sprite>($"Art/Billinear/Bullets/{unitLv}");

        _spriteRenderer.sprite = _sprites[unitLv - 1];

        _targetMonster = targetMonster;
        if (targetMonster == null)
        {
            DestroyBullet();
            return;
        }
        _targetPosition = _targetMonster.transform.position;

        _bulletSpeed = bulletSpeed;

        _ownUnitStatus = Managers.UnitStatus.GetUnitStatus(baseUnit, unitLv);
        wideAttackArea = _ownUnitStatus.wideAttackArea;

        float criticalChance;
        if (wideAttackArea > 0)
        {
            Managers.Sound.Play(Define.SFXNames.AOE);
            Managers.UnitStatus.RuneStatus.AdditionalEffects.
                TryGetValue(AdditionalEffectName.CriticalChanceOfAOE, out criticalChance);
        }
        else
        {
            Managers.Sound.Play(Define.SFXNames.Normal);
            Managers.UnitStatus.RuneStatus.AdditionalEffects.
                TryGetValue(AdditionalEffectName.CriticalChanceOfCommon, out criticalChance);
        }
        isCritical = Random.value <= criticalChance + ConstantData.BaseCriticalChance;
    }

    void Update()
    {
        if (wideAttackArea > 0)
        {
            Vector3 dir = _targetPosition - transform.position;
            transform.SetPositionAndRotation
                (Vector3.MoveTowards(transform.position, _targetPosition, _bulletSpeed * Time.deltaTime),
                Quaternion.FromToRotation(Vector3.up, dir));

            if (Vector3.Distance(gameObject.transform.position, _targetPosition) < 0.01f)
            {
                foreach (Monster monster in Managers.Game.Monsters)
                {
                    if (Vector3.Distance(_targetPosition, monster.transform.position) <= wideAttackArea)
                    {
                        float damageRatio = 1 - CalculateWeightedDistance(_targetPosition,monster.transform.position,wideAttackArea);
                        monster.TakeHit(_ownUnitStatus, isCritical ,damageRatio);
                    }
                }
                GameObject effect = Managers.Resource.Instantiate
                    ("HitEffect_1",Managers.Game.HitEffects);
                effect.GetComponent<HitEffect>().Init(_targetPosition, wideAttackArea);
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
            Vector3 dir = _targetMonster.transform.position - transform.position;
            transform.SetPositionAndRotation
                (Vector3.MoveTowards(transform.position, _targetMonster.transform.position, _bulletSpeed * Time.deltaTime),
                Quaternion.FromToRotation(Vector3.up, dir));
            if (Util.GetDistance(gameObject, _targetMonster.gameObject) < 0.01f)
            {
                _targetMonster.TakeHit(_ownUnitStatus, isCritical);
                GameObject effect = Managers.Resource.Instantiate
                    ("HitEffect_2",Managers.Game.HitEffects);
                effect.GetComponent<HitEffect>().Init(_targetMonster.transform.position);

                DestroyBullet();
            }
        }
    }

    void DestroyBullet()
    {
        Managers.Resource.Destroy(gameObject);
    }

    public float CalculateWeightedDistance(Vector3 currentPosition, Vector3 targetPosition, float maxRange)
    {
        // 현재 위치와 타겟 위치 사이의 거리 계산
        float distance = Vector3.Distance(currentPosition, targetPosition);

        // 최대 공격 범위 이내인 경우에만 가중치 계산
        if (distance <= maxRange)
        {
            // 최대 공격 범위에서의 가중치 (0)
            float maxWeight = 0f;

            // 가장 가까운 거리에서의 가중치 (1)
            float minWeight = 1f;

            // 거리에 대한 가중치 계산
            float weightedDistance = 1f - Mathf.Clamp01(distance / maxRange);

            // 최종 가중치 반환 (minWeight에서 maxWeight까지의 범위에서의 값을 가지도록 보정)
            return Mathf.Lerp(minWeight, maxWeight, weightedDistance);
        }
        else
        {
            // 최대 공격 범위를 벗어난 경우, 최대 가중치 반환
            return 0f;
        }
    }
}
