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
        // ���Ͱ� ���� ����ް� �ִ� ��������� Ž���Ͽ�
        foreach (BaseDebuff monsterDebuff in _monster.Debuffs)
        {
            if (monsterDebuff == this)
                continue;
            // ���ų� �� ���� ������ ������� �ִ��� Ȯ��
            if (monsterDebuff._debuffType == _debuffType)
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
            _monster.QuitStunDebuff();
            EndDebuff();
        }
    }
}
