using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    Monster _monster;
    Image _fill;

    public void InitHPBar(Transform tf)
    {
        _fill = Util.FindChild<Image>(gameObject,"Fill");
        transform.localScale = new Vector3(0.006f, 0.003f, 1f);
        _monster = tf.GetComponent<Monster>();
        SetHpRatio(1f);
        _monster.OnReduceHp += SetHpRatio;
    }

    private void Update()
    {
        transform.position = _monster.transform.position + Vector3.up * 0.4f;
    }

    public void SetHpRatio(float ratio)
    {
        _fill.fillAmount = ratio;
        if (ratio <= 0f)
        {
            Managers.UI.CloseWorldSpaceUI(gameObject);
            _monster.OnReduceHp -= SetHpRatio;
        }
    }

    public override void OnChangeLanguage()
    {
    }

    public override void Init()
    {
        transform.position = new Vector3(540.5f, 964.9f, -91f);
    }
}
