using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunDebuff : BaseDebuff
{
    public void Init(Monster monster, float duration)
    {
        _debuffType = DebuffType.Stun;
        _monster = monster;
        _duration = duration;
        _leftTime = duration;
        _ratio = 0;
        _monster.ApplyStunDebuff();
    }

    protected override void QuitDebuff()
    {
        bool theSameDebuff = false;
        // 몬스터가 현재 적용받고 있는 디버프들을 탐색하여
        foreach (BaseDebuff monsterDebuff in _monster.Debuffs)
        {
            if (monsterDebuff == this)
                continue;
            // 같거나 더 높은 레벨의 디버프가 있는지 확인
            if (monsterDebuff._debuffType == _debuffType)
            {
                theSameDebuff = true;
            }
        }
        // 있다면 그냥 종료
        if (theSameDebuff)
        {
            EndDebuff();
        }
        else
        {
            _monster.QuitStunDebuff();
            EndDebuff();
        }
    }
}
