using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Vector3[] movePoints;
    int nextMovePoint;

    [SerializeField]
    float moveSpeed;

    public void Init(int stageNum, Define.Map map)
    {
        SetMovePoint(map);
        nextMovePoint = 0;

        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        tColor = spr.color;
        // stageNum에 따라서 유닛의 형태, 이동속도, 체력 등 초기화
    }

    private void SetMovePoint(Define.Map map)
    {
        switch (map)
        {
            case Define.Map.Basic:
            {
                movePoints = ConstantData.BasicMapPoint;
            }
            break;
        }
    }

    public void MonsterUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, movePoints[nextMovePoint]) <= 0.01f)
        {
            nextMovePoint++;
            nextMovePoint %= movePoints.Length;
        }
        transform.position = Vector3.MoveTowards(transform.position, movePoints[nextMovePoint], moveSpeed * Time.deltaTime);
    }

    Color tColor;
    public void TakeHit(Color color)
    {
        SpriteRenderer spr = GetComponent<SpriteRenderer>();

        spr.color = color;
        StartCoroutine("CoColor");
    }

    IEnumerator CoColor()
    {
        yield return new WaitForSeconds(0.1f);
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        spr.color = tColor;
    }
}
