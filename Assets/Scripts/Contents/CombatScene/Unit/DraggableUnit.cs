using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUnit : MonoBehaviour
{
    public Action OnDraggableMouseUpEvent;
    public Action OnDraggableMouseClickEvent;
    public Action<Unit> OnDraggableMouseDragEvent;
    Vector3 _mousePos;
    Unit _unit;

    bool _pressed = false;
    float _pressedTime = 0;

    private void Start()
    {
        _unit = GetComponent<Unit>();
    }

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (!_pressed)
        {
            _pressedTime = Time.time;
            _pressed = true;
        }
        _unit.IsDraging = true;
        Util.CheckTheEventAndCall(OnDraggableMouseDragEvent, _unit);
        _mousePos = Input.mousePosition - GetMousePos();
    }

    public void OnMouseDrag()
    {
        if (_pressed)
        {
            float clickTime = 0.2f * Managers.Time.CurTimeScale;
            if (Time.time > _pressedTime + clickTime)
                Managers.Game.UnSelectUnit();
        }
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - _mousePos);
    }

    public void OnMouseUp()
    {
        if (_pressed)
        {
            float clickTime = 0.2f * Managers.Time.CurTimeScale;
            if (Time.time < _pressedTime + clickTime)
            {
                Util.CheckTheEventAndCall(OnDraggableMouseClickEvent);
            }
        }
        _pressed = false;
        _pressedTime = 0;
        Util.CheckTheEventAndCall(OnDraggableMouseUpEvent);

        _unit.IsDraging = false;
    }
}