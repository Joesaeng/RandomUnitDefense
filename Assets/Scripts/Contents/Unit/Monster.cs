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
        // stageNum�� ���� ������ ����, �̵��ӵ�, ü�� �� �ʱ�ȭ
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

        // ��Ʈ ����Ʈ ����(Image, Sound ��)
        // ���÷��� ������
        // �ݺ����� ���鼭 GameManager�� �ִ� ����͵��
        // �Ÿ������ �ؼ� �� �Ÿ��� SplashRange���� �۴ٸ�
        // �� ���͵鿡�� TakeHit �����ϱ�
        // ���÷��������� ���� ����
        // ex) �Ÿ��� ������ 0.2f ���ϸ� 1�� ������,
        // 0.5f ���ϸ� 0.7 ������, 0.5f �̻��̸� 0.4 ������

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
