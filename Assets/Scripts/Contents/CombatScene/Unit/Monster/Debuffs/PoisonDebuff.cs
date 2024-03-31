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
        if(_posionDamageTime >= 0.99f)
        {
            _monster.ReduceHp(_damagePerSecond);
            if (Managers.Game.UnitDPSDict.ContainsKey(Data.UnitNames.PoisonBowMan))
                 Managers.Game.AddDamagesInDPSDict(Data.UnitNames.PoisonBowMan, (int)_damagePerSecond);

            Transform tf = _monster.transform;
            
            GameObject damageTextObj = Managers.Resource.Instantiate("DamageText", tf.position);
            Managers.CompCache.GetOrAddComponentCache(damageTextObj , out DamageText damageTextComp);
            damageTextComp.SetText(_damagePerSecond, isPoison: true);

            _posionDamageTime = 0f;
        }
    }

    protected override void QuitDebuff()
    {
        EndDebuff();
    }
}
