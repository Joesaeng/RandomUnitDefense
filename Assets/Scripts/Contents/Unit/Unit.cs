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

    private void Idle()
    {

    }

    private void Chase()
    {

    }

    protected virtual void Skill()
    {
        
        // ������ �ϴ� Ÿ���� �ٸ��ٵ�,
        // �װŸ� ���⼭ case�� ����� ������ �ϴ°�.
        // �ƴϸ� unit ��ũ��Ʈ�� ��ӹ޴� �ٸ� ���ֵ��� ������ΰ�.
    }

    public void Init(int slotIndex)
    {
        SlotChange(slotIndex);
        gameObject.GetOrAddComponent<DraggableUnit>();
        BindToMouseUp();
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
