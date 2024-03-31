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
    public void InitHPBar(GameObject obj)
    {
        if (_barParent == null)
        {
            _barParent = Util.FindChild(gameObject, "Bar");
        }
        _barParent.SetActive(false);

        if (_fill == null)
        {
            _fill = Util.FindChild<Image>(gameObject, "Fill", true);
        }
        transform.localScale = new Vector3(0.006f, 0.003f, 1f);
        Managers.CompCache.GetOrAddComponentCache(obj, out _monster);
        Managers.CompCache.GetOrAddComponentCache(obj, out Collider2D col);
        _addPositionY = col.bounds.size.y;
        _monster.OnReduceHp += SetHpRatio;
    }

    private void Update()
    {
        Vector3 addPos = Vector3.up * _addPositionY;
        transform.position = _monster.transform.position + addPos;
    }

    public void SetHpRatio(float ratio)
    {
        if (_barParent.activeSelf == false)
            _barParent.SetActive(true);
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
