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
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 시작하는 순간 수평이동이 크면 부모가 드래그 시작한 것, 수직이동이 크면 자식이 드래그 시작한 것
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
