using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackRange : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ActiveAttackRange(Unit unit)
    {
        gameObject.SetActive(true);

        // 공격 범위 크기
        float diameter = unit.StateMachine.AttackRange * 2.0f;
        transform.localScale = Vector3.one * diameter;
        transform.position = unit.transform.position;
    }

    public void UnActiveAttackRange()
    {
        gameObject.SetActive(false);
    }
}
