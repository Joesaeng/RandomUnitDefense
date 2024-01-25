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
        Idle,       // 대기 상태
        Chase,      // 몬스터 추적 상태
        Skill,      // 스킬 사용
    }

    public int ID { get; private set; }
    public int Lv { get; private set; }

    [SerializeField]
    private UnitStateMachine _stateMachine;

    private int _slotIndex;
    private int _moveSlotIndex;


    private bool _isDraging;
    public bool IsDraging { get { return _isDraging; } set { _isDraging = value; } }

    public void UnitUpdate()
    {
        StateUpdate();
    }

    private void StateUpdate()
    {
        if (_isDraging == true)  // 드래그중인 유닛은 아무런 활동을 하지 않음
            return;
        _stateMachine.OnUpdate();
    }

    public void Init(int slotIndex, int id, int level)
    {
        IsDraging = false;
        ID = id;
        Lv = level;
        SlotChange(slotIndex);
        gameObject.GetOrAddComponent<DraggableUnit>();
        _stateMachine = new UnitStateMachine(gameObject, id, level);
        BindToMouseEvent();
    }

    public UnitStat_Base GetUnitStatus()
    {
        return _stateMachine.Stat;
    }    
    public BaseUnits GetBaseUnit()
    {
        return _stateMachine.GetBaseUnit;
    }    
    public void SlotChange(int slotIndex)
    {
        this._slotIndex = slotIndex;
    }

    private void BindToMouseEvent()
    {
        DraggableUnit draggableUnit = gameObject.GetComponent<DraggableUnit>();
        draggableUnit.OnMouseUpEvent -= MouseUpEventReader;
        draggableUnit.OnMouseUpEvent += MouseUpEventReader;
        draggableUnit.OnMouseClickEvent -= MouseClickEventReader;
        draggableUnit.OnMouseClickEvent += MouseClickEventReader;
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
    // 유닛을 드래그 한 후 드롭 할 때 호출되는 메서드
    private void MouseUpEventReader()
    {
        // 이동할 수 없을 때
        if (_moveSlotIndex == _slotIndex || _moveSlotIndex == -1)
        {
            // 원래 있던 위치로 다시 이동
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
