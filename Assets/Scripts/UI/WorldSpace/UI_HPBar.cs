using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    Monster _monster;
    Image _fill;
    GameObject _barParent;
    float _addPositionY = 0f;
    public void InitHPBar(Transform tf)
    {
        if(_barParent == null)
        {
            _barParent = Util.FindChild(gameObject, "Bar");
        }
        _barParent.SetActive(true);
        
        _fill = Util.FindChild<Image>(gameObject,"Fill", true);
        transform.localScale = new Vector3(0.006f, 0.003f, 1f);
        _monster = tf.GetComponent<Monster>();
        Collider2D col = tf.GetComponent<Collider2D>();
        _addPositionY = col.bounds.size.y;
        SetHpRatio(1f);
        _monster.OnReduceHp += SetHpRatio;
    }

    private void Update()
    {
        transform.position = _monster.transform.position + Vector3.up * _addPositionY;
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

    public void OwnMonsterDie()
    {
        _barParent.gameObject.SetActive(false);
    }

    public override void OnChangeLanguage()
    {
    }

    public override void Init()
    {
    }
}
