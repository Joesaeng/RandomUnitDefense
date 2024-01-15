using Data;
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

    private UnitStat stat;
    public UnitStat Stat { get { return stat; } }

    float curSkillCoolTime;

    private Monster target;

    [SerializeField]
    int slotIndex;
    [SerializeField]
    int moveSlotIndex;


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
            if (Util.GetDistance(obj, gameObject) <= Stat.skillRange)
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
        if(target == null || 
            (target != null && Util.GetDistance(target.gameObject,gameObject) > Stat.skillRange))
        {
            MonsterScan();
        }
        if(target != null && curSkillCoolTime > Stat.skillCoolTime)
        {
            State = UnitState.Skill;
        }
    }

    protected virtual void Skill()
    {
        Debug.Log($"{gameObject.name}'s Skill!");
        target.TakeHit(GetComponent<SpriteRenderer>().color, Stat.attackDamage);
        curSkillCoolTime = 0f;
        State = UnitState.Chase;
        // ������ �ϴ� Ÿ���� �ٸ��ٵ�,
        // �װŸ� ���⼭ case�� ����� ������ �ϴ°�.
        // �ƴϸ� unit ��ũ��Ʈ�� ��ӹ޴� �ٸ� ���ֵ��� ������ΰ�.
    }

    public void Init(int slotIndex, int id)
    {
        SlotChange(slotIndex);
        gameObject.GetOrAddComponent<DraggableUnit>();
        stat = Managers.Data.StatDict[id];
        BindToMouseUp();

        // TEMP
        State = UnitState.Chase;
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
