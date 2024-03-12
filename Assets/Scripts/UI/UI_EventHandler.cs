using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour , IDragHandler, IPointerClickHandler
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        Util.CheckTheEventAndCall(OnClickHandler, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Util.CheckTheEventAndCall(OnDragHandler, eventData);
    }

}
