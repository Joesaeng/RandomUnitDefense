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

    // �巡�� �߿��� �ൿ�� �Ұ����ϰ� ����µ� �ʿ�, ���� State�� �ٲ� ��?
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
            // moveSlotIndex�� �������� �̵� �������� Ȯ��
            // �̵� �� slotIndex�� moveSlotIndex��,
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("BorderLine"))
        {
            moveSlotIndex = -1;
        }
    }
    // ������ �巡�� �� �� ��� �� �� ȣ��Ǵ� �޼���
    private void MouseUpEventReader()
    {
        // �̵��� �� ���� ��
        if(moveSlotIndex == slotIndex || moveSlotIndex == -1)
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
