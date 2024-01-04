using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    int slotIndex;
    [SerializeField]
    int moveSlotIndex;

    // 드래그 중에는 행동을 불가능하게 만드는데 필요, 추후 State로 바꿀 듯?
    private bool isDraging;
    public bool IsDraging { get { return isDraging; } set { isDraging = value; } }

    private void Start()
    {
        
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
        DraggableUnit dragAndDrop = gameObject.GetComponent<DraggableUnit>();
        dragAndDrop.OnMouseUpEvent -= MouseUpEventReader;
        dragAndDrop.OnMouseUpEvent += MouseUpEventReader;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("UnitSlot"))
        {
            UnitSlot slot = collision.gameObject.GetComponent<UnitSlot>();
            moveSlotIndex = slot.slotIndex;
            // moveSlotIndex의 슬롯으로 이동 가능한지 확인
            // 이동 후 slotIndex를 moveSlotIndex로,
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("BorderLine"))
        {
            moveSlotIndex = -1;
        }
    }
    // 유닛을 드래그 한 후 드롭 할 때 호출되는 메서드
    private void MouseUpEventReader()
    {
        // 이동할 수 없을 때
        if(moveSlotIndex == slotIndex || moveSlotIndex == -1)
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
