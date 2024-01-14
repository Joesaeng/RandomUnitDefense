using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitState
    {
        Idle,       // ���� ���� ���� ���Ͱ� ���� ��
        Chase,      // ���� ���� ���� ���Ͱ� �ְ�, ��ų ��Ÿ�� �� ��,
        Skill,      // ������ ��ų ���
    }

    private UnitState state;
    public UnitState State { get { return state; } set { state = value; } }

    private Monster target;

    [SerializeField]
    int slotIndex;
    [SerializeField]
    int moveSlotIndex;

    // TEMP
    [SerializeField]
    float skillRange;
    [SerializeField]
    float skillCoolTime;
    [SerializeField]
    float curSkillCoolTime;
    [SerializeField]
    int attackDamage = 5;

    //
    private bool isDraging;
    public bool IsDraging { get { return isDraging; } set { isDraging = value; } }

    public void UnitUpdate()
    {
        StateUpdate();
    }

    private void StateUpdate()
    {
        if (isDraging == true)  // �巡������ ������ �ƹ��� Ȱ���� ���� ����
                                // �巡�� �߿� ��ų ��Ÿ�� ���� �Ͻ����� �ؾ��� ��?
            return;
        curSkillCoolTime += Time.deltaTime;
        switch (State)
        {
            case UnitState.Idle:
                Idle();
                break;
            case UnitState.Chase:
                Chase();
                break;
            case UnitState.Skill:
                Skill();
                break;
        }
    }

    private void MonsterScan()
    {
        foreach (GameObject obj in Managers.Game.Monsters)
        {
            if (Util.GetDistance(obj, gameObject) <= skillRange)
            {
                if(target == null)
                    target = obj.GetComponent<Monster>();
                else if (Util.GetDistance(target.gameObject, gameObject)
                    > Util.GetDistance(obj, gameObject))
                {
                    target = obj.GetComponent<Monster>();
                }
            }
        }
        // ���� ���� ���� �ִ� Ÿ���� ���Ͽ� �����Ѵ�.
        // Ÿ���� ���� ������ �����ų�, Ÿ�� ��� ��
        // �ٸ� ���ͷ� Ÿ���� �����Ѵ�.
        // Ÿ���� ���� �� Idle�� ���� ���� // ���� Idle�� �� �ʿ䰡 ������?
    }

    private void Idle()
    {
        // Ÿ���� ���� ��
    }

    protected virtual void Chase()
    {
        // 
        if(target == null || Util.GetDistance(target.gameObject,gameObject) > skillRange)
        {
            MonsterScan();
        }
        if(target != null && curSkillCoolTime > skillCoolTime)
        {
            State = UnitState.Skill;
        }
        // Scan���� ���� targets �� ��� �༮ �� Ÿ������ �Ұ��ΰ�?
        // ���������� ������ ������
        // �ϴ� �����ϴ� ���� ���ݹ��� ���� ���� �� �� �༮�� �����ϰ� �ؾ���.

    }

    protected virtual void Skill()
    {
        Debug.Log($"{gameObject.name}'s Skill!");
        target.TakeHit(GetComponent<SpriteRenderer>().color, attackDamage);
        curSkillCoolTime = 0f;
        State = UnitState.Chase;
        // ������ �ϴ� Ÿ���� �ٸ��ٵ�,
        // �װŸ� ���⼭ case�� ����� ������ �ϴ°�.
        // �ƴϸ� unit ��ũ��Ʈ�� ��ӹ޴� �ٸ� ���ֵ��� ������ΰ�.
    }

    public void Init(int slotIndex)
    {
        SlotChange(slotIndex);
        gameObject.GetOrAddComponent<DraggableUnit>();
        BindToMouseUp();

        // TEMP
        State = UnitState.Chase;
        skillRange = 3f;
        skillCoolTime = 1f;
    }

    public void SlotChange(int slotIndex)
    {
        this.slotIndex = slotIndex;
    }

    private void BindToMouseUp()
    {
        DraggableUnit draggableUnit = gameObject.GetComponent<DraggableUnit>();
        draggableUnit.OnMouseUpEvent -= MouseUpEventReader;
        draggableUnit.OnMouseUpEvent += MouseUpEventReader;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("UnitSlot"))
        {
            UnitSlot slot = collision.gameObject.GetComponent<UnitSlot>();
            moveSlotIndex = slot.slotIndex;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BorderLine"))
        {
            moveSlotIndex = -1;
        }
    }
    // ������ �巡�� �� �� ��� �� �� ȣ��Ǵ� �޼���
    private void MouseUpEventReader()
    {
        // �̵��� �� ���� ��
        if (moveSlotIndex == slotIndex || moveSlotIndex == -1)
        {
            // ���� �ִ� ��ġ�� �ٽ� �̵�
            Managers.Game.MoveUnitBetweenSlots(slotIndex, slotIndex);
            moveSlotIndex = -1;
            return;
        }
        Managers.Game.MoveUnitBetweenSlots(slotIndex, moveSlotIndex);
        moveSlotIndex = -1;
    }

}
