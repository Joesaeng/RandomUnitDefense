using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    enum GameObjects
    {
        Fill
    }

    Monster _monster;
    Image _fill;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        _monster = transform.parent.GetComponent<Monster>();
        if (_fill == null)
            _fill = GetObject((int)GameObjects.Fill).GetComponent<Image>();
    }

    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * 0.7f;

        float ratio = (float)_monster.CurHp / (float)_monster.MaxHp;
        SetHpRatio(ratio);
    }

    public void SetHpRatio(float ratio)
    {
        _fill.fillAmount = ratio;
    }

    public override void OnChangeLanguage()
    {
    }
}
