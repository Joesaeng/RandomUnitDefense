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
        if (isDraging == true)  // 드래그중인 유닛은 아무런 활동을 하지 않음
                                // 드래그 중에 스킬 쿨타임 등은 일시정지 해야할 듯?
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
        // 공격 범위 내에 있는 타겟을 정하여 저장한다.
        // 타겟이 범위 밖으로 나가거나, 타겟 사망 시
        // 다른 몬스터로 타겟을 변경한다.
        // 타겟이 없을 때 Idle로 상태 변경 // 굳이 Idle로 갈 필요가 있을까?
    }

    private void Idle()
    {
        // 타겟이 없을 때
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
        // Scan에서 잡힌 targets 중 어떠한 녀석 을 타겟으로 할것인가?
        // 컨텐츠적인 문제긴 하지만
        // 일단 공격하던 적이 공격범위 내에 있을 때 그 녀석을 공격하게 해야함.

    }

    protected virtual void Skill()
    {
        Debug.Log($"{gameObject.name}'s Skill!");
        target.TakeHit(GetComponent<SpriteRenderer>().color, attackDamage);
        curSkillCoolTime = 0f;
        State = UnitState.Chase;
        // 공격을 하는 타입이 다를텐데,
        // 그거를 여기서 case로 나누어서 진행을 하는가.
        // 아니면 unit 스크립트를 상속받는 다른 유닛들을 만들것인가.
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
