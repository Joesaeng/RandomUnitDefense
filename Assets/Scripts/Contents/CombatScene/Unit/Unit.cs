using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int ID { get; private set; }
    public int Lv { get; private set; }

    private UnitStateMachine _stateMachine;
    public UnitStateMachine StateMachine { get { return _stateMachine; } }

    private int _slotIndex;
    public int SlotIndex => _slotIndex;
    private int _moveSlotIndex;

    private bool _isDraging;
    public bool IsDraging { get { return _isDraging; } set { _isDraging = value; } }

    public void UnitUpdate()
    {
        StateUpdate();
    }

    private void StateUpdate()
    {
        if (_isDraging == true)  // �巡������ ������ �ƹ��� Ȱ���� ���� ����
            return;
        _stateMachine.OnUpdate();
    }

    public void Init(int slotIndex, int id, int level,string unitname)
    {
        IsDraging = false;
        ID = id;
        Lv = level;
        SlotChange(slotIndex);
        gameObject.GetOrAddComponent<DraggableUnit>();
        _stateMachine = gameObject.GetOrAddComponent<UnitStateMachine>();
        _stateMachine.Init(gameObject, ID, Lv, unitname);
        BindEvent();
    }

    public UnitNames GetBaseUnit()
    {
        return _stateMachine.BaseUnit;
    }    

    public void SlotChange(int slotIndex)
    {
        this._slotIndex = slotIndex;
    }

    private void BindEvent()
    {
        DraggableUnit draggableUnit = gameObject.GetComponent<DraggableUnit>();
        draggableUnit.OnDraggableMouseUpEvent -= MouseUpEventReader;
        draggableUnit.OnDraggableMouseUpEvent += MouseUpEventReader;

        draggableUnit.OnDraggableMouseClickEvent -= MouseClickEventReader;
        draggableUnit.OnDraggableMouseClickEvent += MouseClickEventReader;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("UnitSlot"))
        {
            UnitSlot slot = collision.gameObject.GetComponent<UnitSlot>();
            _moveSlotIndex = slot.slotIndex;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BorderLine"))
        {
            _moveSlotIndex = -1;
        }
    }
    // ������ �巡�� �� �� ��� �� �� ȣ��Ǵ� �޼���
    private void MouseUpEventReader()
    {
        // �̵��� �� ���� ��
        if (_moveSlotIndex == _slotIndex || _moveSlotIndex == -1)
        {
            // ���� �ִ� ��ġ�� �ٽ� �̵�
            Managers.Game.MoveUnitBetweenSlots(_slotIndex, _slotIndex);
            _moveSlotIndex = -1;
            return;
        }
        Managers.Game.MoveUnitBetweenSlots(_slotIndex, _moveSlotIndex);
        _moveSlotIndex = -1;
    }

    private void MouseClickEventReader()
    {
        Managers.Game.SelectUnit(gameObject);
    }
}
