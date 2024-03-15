using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollScript : ScrollRect
{
    bool forParent;
    NestedScrollManager NM;
    ScrollRect parentScrollRect;
    protected override void Start()
    {
        
    }
    public void Init()
    {
        NM = GameObject.FindWithTag("NestedScrollManager").GetComponent<NestedScrollManager>();
        parentScrollRect = GameObject.FindWithTag("NestedScrollManager").GetComponent<ScrollRect>();

        content = GameObject.Find("UnitSlots").GetComponent<RectTransform>();
        horizontal = false;
        vertical = true;
        movementType = MovementType.Clamped;
        inertia = true;
        decelerationRate = 0.135f;
        scrollSensitivity = 1;
        viewport = GameObject.Find("UnitSlotsViewport").GetComponent<RectTransform>();
        horizontalScrollbar = null;
        verticalScrollbar = GameObject.Find("UnitSlotsScrollBar").GetComponent<Scrollbar>();
        verticalScrollbarVisibility = ScrollbarVisibility.AutoHideAndExpandViewport;
        verticalScrollbarSpacing = 0;
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        // �巡�� �����ϴ� ���� �����̵��� ũ�� �θ� �巡�� ������ ��, �����̵��� ũ�� �ڽ��� �巡�� ������ ��
        forParent = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);

        if(forParent)
        {
            NM.OnBeginDrag(eventData);
            parentScrollRect.OnBeginDrag(eventData);
        }
        else
            base.OnBeginDrag(eventData);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            NM.OnDrag(eventData);
            parentScrollRect.OnDrag(eventData);
        }
        else
            base.OnDrag(eventData);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            NM.OnEndDrag(eventData);
            parentScrollRect.OnEndDrag(eventData);
        }
        else
            base.OnEndDrag(eventData);
    }
}
