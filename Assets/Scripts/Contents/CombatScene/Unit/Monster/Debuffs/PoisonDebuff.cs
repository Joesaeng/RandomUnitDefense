using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : BaseDebuff
{
    public float _damagePerSecond;
    public float _posionDamageTime;
    public void Init(Monster monster, float duration, float damagePerSecond)
    {
        _debuffType = DebuffType.Poison;
        _monster = monster;
        _duration = duration;
        _leftTime = duration;
        _ratio = 0;
        _damagePerSecond = damagePerSecond;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        _posionDamageTime += Time.deltaTime;
        _monster.ReduceHp(_damagePerSecond * Time.deltaTime);
    }

    protected override void QuitDebuff()
    {
        EndDebuff();
    }
}
