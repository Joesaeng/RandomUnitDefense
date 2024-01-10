using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitState
    {
        Idle,       // 공격 범위 내에 몬스터가 없을 때
        Chase,      // 공격 범위 내에 몬스터가 있고, 스킬 쿨타임 일 때,
        Skill,      // 유닛의 스킬 사용
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
        if (isDraging == true)  // 드래그중인 유닛은 아무런 활동을 하지 않음
                                // 드래그 중에 스킬 쿨타임 등은 일시정지 해야할 듯?
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
        
        // 공격을 하는 타입이 다를텐데,
        // 그거를 여기서 case로 나누어서 진행을 하는가.
        // 아니면 unit 스크립트를 상속받는 다른 유닛들을 만들것인가.
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
    // 유닛을 드래그 한 후 드롭 할 때 호출되는 메서드
    private void MouseUpEventReader()
    {
        // 이동할 수 없을 때
        if (moveSlotIndex == slotIndex || moveSlotIndex == -1)
        {
            // 원래 있던 위치로 다시 이동
            Managers.Game.MoveUnitBetweenSlots(slotIndex, slotIndex);
            moveSlotIndex = -1;
            return;
        }
        Managers.Game.MoveUnitBetweenSlots(slotIndex, moveSlotIndex);
        moveSlotIndex = -1;
    }

}
