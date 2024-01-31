using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DebuffType
{
    Slow,
    Poison,
    Stun,
    Count,
}

public abstract class BaseDebuff
{
    public DebuffType _debuffType;
    public float _duration;
    public float _leftTime;
    public float _ratio;
    public Monster _monster;

    public virtual void OnUpdate()
    {
        _leftTime -= Time.deltaTime;
        if (_leftTime < 0)
            QuitDebuff();
    }

    protected abstract void QuitDebuff();

    protected void EndDebuff()
    {
        if (_monster != null)
            _monster.QuitDebuff(this);
    }
}
