using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Vector3[] movePoints;
    int nextMovePoint;

    [SerializeField]
    float moveSpeed;
    
    int maxHp = 10;
    int curHp;

    public void Init(int stageNum, Define.Map map)
    {
        SetMovePoint(map);
        nextMovePoint = 0;

        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        tColor = new Color(1f,56/255f,0f);
        spr.color = tColor;

        curHp = maxHp;
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
    public void TakeHit(Color color, int damage)
    {
        SpriteRenderer spr = GetComponent<SpriteRenderer>();

        spr.color = color;

        // 히트 이펙트 실행(Image, Sound 등)
        // 스플래쉬 데미지
        // 반복문을 돌면서 GameManager에 있는 몬슽터들과
        // 거리계산을 해서 그 거리가 SplashRange보다 작다면
        // 그 몬스터들에게 TakeHit 실행하기
        // 스플래쉬데미지 계산식 적용
        // ex) 거리가 비율상 0.2f 이하면 1의 데미지,
        // 0.5f 이하면 0.7 데미지, 0.5f 이상이면 0.4 데미지

        curHp -= damage;
        if(curHp <= 0)
        {
            Managers.Game.ThisFrameDieMonster(this.gameObject);
            StopAllCoroutines();
        }
        else
        {
            // TEMP
            StartCoroutine("CoColor");
        }
    }

    IEnumerator CoColor()
    {
        yield return new WaitForSeconds(0.1f);
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        spr.color = tColor;
    }
}
