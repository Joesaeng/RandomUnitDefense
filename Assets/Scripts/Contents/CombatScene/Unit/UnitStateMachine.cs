using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Unit;

public enum UnitState
{
    SearchTarget,           // 타겟 스캔
    SkillState,             // 스킬 사용
}



[Serializable]
public class UnitStateMachine : MonoBehaviour
{
    GameObject _ownObj;

    UnitAnimaitor _unitAnimator;

    Monster _targetMonster;

    Animator _animator;

    UnitStatus _unitStatus;

    private Action<Define.UnitAnimationState> OnAnimStateChanged;
    private Define.UnitAnimationState _currentAnimState;
    public Define.UnitAnimationState CurrentAnimState
    {
        get => _currentAnimState;
        set
        {
            Util.CheckTheEventAndCall(OnAnimStateChanged, value);
            _currentAnimState = value;
        }
    }


    private UnitNames       _baseUnit;
    private string          _job;
    private UnitType        _attackType;
    private int             _unitId;
    private int             _unitLv;
    private float           _attackRange;

    public float AttackRange => _unitStatus.attackRange;
    public UnitNames BaseUnit => _baseUnit;

    private UnitState _state;

    [SerializeField]
    float _curAttackRateTime;

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

    public void Init(GameObject ownObj, int unitId, int level, string unitname)
    {
        _ownObj = ownObj;
        _baseUnit = (UnitNames)unitId;
        _unitId = unitId;
        _unitLv = level;

        if (_unitAnimator != null)
        {
            Managers.Resource.Destroy(_unitAnimator.gameObject);
        }
        {
            // 애니메이터 프리펩 초기화
            GameObject go = Managers.Resource.Instantiate($"Units/{unitname}_{level}",_ownObj.transform);
            go.transform.localPosition = new Vector3(0, -0.2f, 0);
            go.transform.localScale = new Vector3(1.3f, 1.3f, 1);
            _unitAnimator = go.GetOrAddComponent<UnitAnimaitor>();
            _unitAnimator.Init();
            _animator = Util.FindChild<Animator>(go);
        }

        _job = Managers.Data.BaseUnitDict[unitId].job.ToString();

        UpdateUnitStatus();

        Managers.InGameItem.OnCalculateEquipItem -= UpdateUnitStatus;
        Managers.InGameItem.OnCalculateEquipItem += UpdateUnitStatus;
        Managers.UnitStatus.OnUnitUpgrade -= UpdateUnitStatus;
        Managers.UnitStatus.OnUnitUpgrade += UpdateUnitStatus;

        _attackType = _unitStatus.unitType;
        _attackRange = _unitStatus.attackRange;

        _curAttackRateTime = 10f; // 생성되자마자 공격할수 있게

        ChangeState(UnitState.SearchTarget);
        PlayStateAnimation(CurrentAnimState);
    }

    private void UpdateUnitStatus()
    {
        _unitStatus = Managers.UnitStatus.GetUnitStatus(_baseUnit, _unitLv);
    }

    public void ChangeState(UnitState state)
    {
        _state = state;
        switch (_state)
        {
            case UnitState.SearchTarget:
                CurrentAnimState = Define.UnitAnimationState.Idle;
                break;
            case UnitState.SkillState:
                CurrentAnimState = Define.UnitAnimationState.Attack;
                break;
        }
    }

    private void PlayStateAnimation(Define.UnitAnimationState animstate)
    {
        if (animstate == Define.UnitAnimationState.Attack)
        {
            _unitAnimator.PlayAnimation($"Attack_{_job}");
        }
        else
        {
            _unitAnimator.PlayAnimation($"{animstate.ToString()}");
        }
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

        if (_curAttackRateTime > _unitStatus.attackRate)
            Skill();
    }

    private void Skill()
    {
        float flipX = transform.position.x < _targetMonster.transform.position.x ? -1.3f : 1.3f;
        _unitAnimator.transform.localScale = new Vector3(flipX, 1.3f, 1);
        float attackAnimSpeed = 1 / _unitStatus.attackRate;
        attackAnimSpeed = attackAnimSpeed < 1 ? 1 : attackAnimSpeed;
        _animator.SetFloat("AttackAnimSpeed", attackAnimSpeed);

        _unitAnimator.PlayAnimation($"Attack_{_job}");

        float bullettime = ConstantData.UnitAttackAnimLength / attackAnimSpeed * 0.66f;
        Invoke("CreateBullet", bullettime);
        _curAttackRateTime = 0f;
    }

    public void CreateBullet()
    {
        GameObject bullet = Managers.Game.Spawn("UnitBullet");
        bullet.transform.position = _ownObj.transform.position;
        bullet.GetComponent<UnitBullet>().Init(_targetMonster, _baseUnit, _unitLv);
    }
}
