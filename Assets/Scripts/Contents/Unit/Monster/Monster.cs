using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster : MonoBehaviour
{
    MonsterData _monsterStat = new MonsterData();

    Vector3[] _movePoints;
    int _nextMovePoint;

    [SerializeField]
    private bool _isStun { get; set; }  = false;
    [SerializeField]
    private bool _isDead = false;
    public bool IsDead => _isDead;

    [SerializeField]
    List<BaseDebuff> _debuffs;
    public List<BaseDebuff> Debuffs => _debuffs;

    // Stats
    float _moveSpeed;
    float _curMoveSpeed;
    float _curHp;
    public float CurHp => _curHp;
    public float MaxHp { get;private set; }
    int _defense;

    public void Init(int stageNum, Define.Map map)
    {
        SetMovePoint(map);
        _nextMovePoint = 0;
        _monsterStat = Managers.Data.GetMonsterData(stageNum);
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        tColor = new Color(1f, 56 / 255f, 0f);
        spr.color = tColor;

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);

        // stageNum�� ���� ������ ����, �̵��ӵ�, ü�� �� �ʱ�ȭ
        MaxHp = _monsterStat.maxHp;
        _curHp = _monsterStat.maxHp;
        _moveSpeed = _monsterStat.moveSpeed;
        _curMoveSpeed = _moveSpeed;
        _defense = _monsterStat.defense;

        _debuffs = new List<BaseDebuff>();

        _debuffs.Clear();

        _isDead = false;
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
        for(int i = 0; i < Debuffs.Count; ++i)
        {
            Debuffs[i].OnUpdate();
        }
        
        Move();
    }

    #region �����

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
    }

    public void QuitStunDebuff()
    {
        _isStun = false;
    }

    private void ApplyDebuff(BaseDebuff debuff)
    {
        _debuffs.Add(debuff);
    }

    private void QuitAllDebuff()
    {
        _isStun = false;
        for(int i = 0; i < Debuffs.Count;++i)
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
            _nextMovePoint++;
            _nextMovePoint %= _movePoints.Length;
        }
        transform.position = Vector3.MoveTowards(transform.position, _movePoints[_nextMovePoint], _curMoveSpeed * Time.deltaTime);
    }

    Color tColor;
    public void TakeHit(Color color, UnitStat_Base stat, Define.AttackType attackType = Define.AttackType.Common)
    {
        if (IsDead)
            return;
        SpriteRenderer spr = GetComponent<SpriteRenderer>();

        spr.color = color;

        // ��Ʈ ����Ʈ ����(Image, Sound ��)

        switch (attackType)
        {
            case Define.AttackType.SlowMagic:
            {
                if (stat is SlowMagician Stat)
                {
                    SlowDebuff slowDebuff = new SlowDebuff();
                    slowDebuff.Init(this, Stat.slowDuration, Stat.slowRatio);
                    ApplyDebuff(slowDebuff);
                }
                break;
            }
            case Define.AttackType.Stun:
            {
                if (stat is StunGun Stat)
                {
                    StunDebuff stunDebuff = new StunDebuff();
                    stunDebuff.Init(this, Stat.stunDuration);
                    ApplyDebuff(stunDebuff);
                }
                break;
            }
            case Define.AttackType.Poison:
            {
                if (stat is PoisonBowMan Stat)
                {
                    PoisonDebuff poisonDebuff = new PoisonDebuff();
                    poisonDebuff.Init(this, Stat.poisonDuration,Stat.poisonDamagePerSecond);
                    ApplyDebuff(poisonDebuff);
                }
                break;
            }
        }

        // ������ ���ݷ��� ������ ���º��� ���� ��� 1�� �������� �ް� �մϴ�.
        float damage = stat.attackDamage - _defense > 1 ? stat.attackDamage - _defense : 1;
        ReduceHp(damage);
        if (_curHp <= 0)
        {
            
        }
        else
        {
            // TEMP
            StartCoroutine("CoColor");
        }
    }

    public void ReduceHp(float damage)
    {
        _curHp -= damage;
        if (_curHp <= 0)
        {
            _isDead = true;
            Managers.Game.RegisterDyingMonster(this.gameObject);
            QuitAllDebuff();
            StopAllCoroutines();
        }
    }

    IEnumerator CoColor()
    {
        yield return new WaitForSeconds(0.1f);
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        spr.color = tColor;
    }
}
