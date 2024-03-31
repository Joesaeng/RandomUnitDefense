using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class Monster : MonoBehaviour
{
    UnitAnimator _unitAnimator;

    Transform _animatorTF;

    UI_HPBar _hpBar;

    Vector3[] _movePoints;
    int _nextMovePoint;
    int _previousMovePoint;

    [SerializeField]
    private bool _isStun { get; set; } = false;
    [SerializeField]
    private bool _isDead = false;
    public bool IsDead => _isDead;

    [SerializeField]
    List<BaseDebuff> _debuffs;
    public List<BaseDebuff> Debuffs => _debuffs;

    public Action<float> OnReduceHp; // HpRatio

    // Stats
    float _moveSpeed;
    float _curMoveSpeed;
    float _curHp;
    public float CurHp => _curHp;
    public float MaxHp { get; private set; }

    int _defense;

    int _givenRuny;
    public int GivenRuny { get => _givenRuny;}

    float _damageTextPosUp;

    public void Init(int stageNum, Define.Map map)
    {
        SetMovePoint(map);
        _nextMovePoint = 0;
        _previousMovePoint = 3;

        _hpBar = Managers.UI.MakeWorldSpaceUI<UI_HPBar>(Managers.Game.HpBarPanel);
        _hpBar.InitHPBar(gameObject);

        if (_unitAnimator != null)
        {
            Managers.Resource.Destroy(_unitAnimator.gameObject);
        }
        {
            // 애니메이터 프리펩 초기화
            string stage = stageNum.ToString();
            string tstage = stage.PadLeft(3, '0');

            GameObject go = Managers.Resource.Instantiate($"Units/Monster{tstage}",transform);
            _animatorTF = go.transform;
            _animatorTF.localPosition = new Vector3(0, -0.5f, 0);
            _animatorTF.localScale = new Vector3(1.25f, 1.25f, 1);
            Managers.CompCache.GetOrAddComponentCache(go, out _unitAnimator);
            _unitAnimator.Init();
        }

        _unitAnimator.PlayAnimation("Run");

        // stageNum에 따라서 유닛의 형태, 이동속도, 체력 등 초기화
        if(Managers.Data.StageDict.TryGetValue(stageNum, out StageData stageData) == false)
        {
            Debug.Log("Can not Find StageData");
            return;
        }
        MaxHp = stageData.monsterHp;
        _curHp = MaxHp;
        _moveSpeed = stageData.monstermoveSpeed;
        _curMoveSpeed = _moveSpeed;
        _defense = stageData.monsterdefense;
        _givenRuny = stageData.givenRuby;

        float reduceDefence = _defense;
        if(Managers.UnitStatus.RuneStatus.BaseRuneEffects.TryGetValue(BaseRune.Curse, out float curseRuneValue))
        {
            reduceDefence *= (1 - curseRuneValue);
        }

        _defense = (int)reduceDefence;

        _debuffs = new List<BaseDebuff>();

        _debuffs.Clear();

        _isDead = false;

        Collider2D col = GetComponent<Collider2D>();
        _damageTextPosUp = col.bounds.size.y;
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
        _unitAnimator.PlayAnimation("Debuff_Stun");
    }

    public void QuitStunDebuff()
    {
        _isStun = false;
        _unitAnimator.PlayAnimation("Run");
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
            _previousMovePoint = _nextMovePoint;
            _nextMovePoint++;
            _nextMovePoint %= _movePoints.Length;
            float fiipX;
            if (_movePoints[_previousMovePoint].x != _movePoints[_nextMovePoint].x)
            {
                fiipX = _movePoints[_previousMovePoint].x < _movePoints[_nextMovePoint].x ? -1.25f : 1.25f;
                _animatorTF.localScale = new Vector3(fiipX, 1.25f, 1);
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, _movePoints[_nextMovePoint], _curMoveSpeed * Time.deltaTime);

    }

    public void TakeHit(UnitStatus attackerStat,bool isCritical ,float damageRatio = 1)
    {
        if (IsDead)
            return;
        UnitNames unit = attackerStat.unit;

        // 디버프 유닛에게 피격 시 디버프 적용
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

        // 치명타 피해 적용
        float addCriticalDamage = 0f;
        if(attackerStat.unitType == UnitType.Common ||
            unit == UnitNames.StunGun || unit == UnitNames.PoisonBowMan)
        {
            Managers.UnitStatus.RuneStatus.AdditionalEffects.
                TryGetValue(AdditionalEffectName.CriticalDamageOfCommon, out addCriticalDamage);
        }
        else
        {
            Managers.UnitStatus.RuneStatus.AdditionalEffects.
                TryGetValue(AdditionalEffectName.CriticalDamageOfAOE, out addCriticalDamage);
        }

        float damage = attackerStat.attackDamage * damageRatio;

        if (isCritical)
            damage *= (ConstantData.BaseCriticalDamageRatio + addCriticalDamage);

        // 유닛의 공격력이 몬스터의 방어력보다 낮은 경우 1의 데미지를 받게 합니다.
        damage = damage - _defense > 1 ? damage - _defense : 1;

        float addedDamageOfRune;
        Managers.UnitStatus.RuneStatus.AdditionalEffects.
                TryGetValue(AdditionalEffectName.AddedDamage, out addedDamageOfRune);

        float addedDamage = (Managers.InGameItem.CurrentStatusOnEquipedItem.addedDamage + addedDamageOfRune)
                            * damageRatio;

        // DPS 측정기
        if (Managers.Game.UnitDPSDict.ContainsKey(unit))
            Managers.Game.AddDamagesInDPSDict(unit,(int)(damage + addedDamage));

        ReduceHp(damage + addedDamage);

        Vector3 pos = transform.position;
        pos.Set(pos.x, pos.y + _damageTextPosUp, pos.z);

        GameObject damageTextObj = Managers.Resource.Instantiate("DamageText", pos);
        Managers.CompCache.GetOrAddComponentCache(damageTextObj, out DamageText damageTextComp);
        
        damageTextComp.SetText(damage + addedDamage, isCritical);

        // GameObject damageText = Managers.Resource.Instantiate("DamageText", pos);
        // damageText.GetComponent<DamageText>().SetText(damage + addedDamage, isCritical);
    }

    public void ReduceHp(float damage)
    {
        if (IsDead)
            return;

        _curHp -= damage;

        float hpRatio = CurHp / MaxHp;
        OnReduceHp.Invoke(hpRatio);
        if (_curHp <= 0)
        {
            _isDead = true;
            Managers.Game.RegisterDyingMonster(this);
            QuitAllDebuff();
            _hpBar.OwnMonsterDie();
        }
    }
}
