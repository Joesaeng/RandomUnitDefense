using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LobbySceneSlot : UI_Base, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    protected NestedScrollManager NM;
    protected ScrollRect baseScrollRect;
    protected ScrollRect parentSlotsScroll;

    protected bool forParent;

    public override void Init()
    {
        NM = GameObject.FindWithTag("NestedScrollManager").GetComponent<NestedScrollManager>();
        baseScrollRect = GameObject.FindWithTag("NestedScrollManager").GetComponent<ScrollRect>();
    }

    public override void OnChangeLanguage()
    {
    }

    // 스크롤의 드래그를 위함
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        forParent = Mathf.Abs(eventData.delta.x) > 1f && Mathf.Abs(eventData.delta.y) > 1f;
        if (forParent)
            forParent = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);

        if (forParent)
        {
            NM.OnBeginDrag(eventData);
            baseScrollRect.OnBeginDrag(eventData);
        }
        else
            parentSlotsScroll.OnBeginDrag(eventData);
    }
    public virtual void OnDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            NM.OnDrag(eventData);
            baseScrollRect.OnDrag(eventData);
        }
        else
            parentSlotsScroll.OnDrag(eventData);
    }
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            NM.OnEndDrag(eventData);
            baseScrollRect.OnEndDrag(eventData);
        }
        else
            parentSlotsScroll.OnEndDrag(eventData);
    }

    private void Start()
    {
        
    }

    private void Awake()
    {
        Init();
    }

    void Update()
    {
        
    }
}
