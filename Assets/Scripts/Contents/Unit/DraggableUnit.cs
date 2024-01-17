using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUnit : MonoBehaviour
{
    public Action OnMouseUpEvent;
    Vector3 _mousePos;
    Unit _unit;

    private void Start()
    {
        _unit = transform.GetComponent<Unit>();
    }

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    public void OnMouseDown()
    {
        _unit.IsDraging = true;
        _mousePos = Input.mousePosition - GetMousePos();
    }

    public void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - _mousePos);
    }

    public void OnMouseUp()
    {
        if (OnMouseUpEvent != null)
            OnMouseUpEvent.Invoke();

        _unit.IsDraging = false;
    }
}
