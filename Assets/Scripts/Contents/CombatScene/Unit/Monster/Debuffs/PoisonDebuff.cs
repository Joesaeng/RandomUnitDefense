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
                Managers.Game.AddDamagesInDPSQueue(Data.UnitNames.PoisonBowMan, (int)_damagePerSecond);
            GameObject damageText = Managers.Resource.Instantiate
                ("DamageText",Managers.Game.DamageTexts);
            Transform tf = _monster.transform;
            Vector3 pos = new Vector3(tf.position.x,tf.position.y ,tf.position.z);
            damageText.GetComponent<DamageText>().SetText(_damagePerSecond, pos, isPoison: true);
            _posionDamageTime = 0f;
        }
    }

    protected override void QuitDebuff()
    {
        EndDebuff();
    }
}
