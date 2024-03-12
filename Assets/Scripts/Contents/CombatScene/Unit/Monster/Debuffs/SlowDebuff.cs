using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDebuff : BaseDebuff
{
    public void Init(Monster monster,float duration, float ratio)
    {
        _debuffType = DebuffType.Slow;
        _monster = monster;
        _duration = duration;
        _leftTime = duration;
        _ratio = ratio;
        _monster.ApplySlowDebuff(ratio);
    }

    protected override void QuitDebuff()
    {
        bool theSameDebuff = false;
        // ���Ͱ� ���� ����ް� �ִ� ��������� Ž���Ͽ�
        foreach(BaseDebuff monsterDebuff in _monster.Debuffs)
        {
            if (monsterDebuff == this)
                continue;
            // ���ų� �� ���� ������ ������� �ִ��� Ȯ��
            if (monsterDebuff._debuffType == _debuffType
                && monsterDebuff._ratio >= _ratio)
            {
                theSameDebuff = true;
            }
        }
        // �ִٸ� �׳� ����
        if (theSameDebuff)
        {
            EndDebuff();
        }
        else
        {
            _monster.QuitSlowDebuff();
            EndDebuff();
        }
    }
}
