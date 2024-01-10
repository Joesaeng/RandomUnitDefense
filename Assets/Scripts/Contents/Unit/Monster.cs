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

    private void Update()
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
}
