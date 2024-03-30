using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUnit : MonoBehaviour
{
    public Action OnDraggableMouseUpEvent;
    public Action OnDraggableClickEvent;
    public Action<Unit> OnDraggableDoubleClickEvent;
    public Action<Unit> OnDraggableMouseDragEvent;
    Vector3 _mousePos;
    Unit _unit;

    bool _pressed = false;
    bool _isDoubleClicked = false;

    float _pressedTime = 0;
    float _doubleClickedTime = -1f;
    float _clickTime = 0.2f;
    float _interval = 0.25f;

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

        if (Time.time - _doubleClickedTime < _interval)
        {
            Util.CheckTheEventAndCall(OnDraggableDoubleClickEvent, _unit);
            _doubleClickedTime = -1f;
            _isDoubleClicked = true;
        }

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
            float clickTime = _clickTime * Managers.Time.CurTimeScale;
            if (Time.time > _pressedTime + clickTime)
                Managers.Game.UnSelectUnit();
        }
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - _mousePos);
    }

    public void OnMouseUp()
    {
        if (Time.time - _doubleClickedTime > _interval)
        {
            _isDoubleClicked = false;
            _doubleClickedTime = Time.time;
        }

        if (_pressed && !_isDoubleClicked)
        {
            float clickTime = _clickTime * Managers.Time.CurTimeScale;
            if (Time.time < _pressedTime + clickTime)
            {
                Util.CheckTheEventAndCall(OnDraggableClickEvent);
            }
        }
        _pressed = false;
        _pressedTime = 0f;
        Util.CheckTheEventAndCall(OnDraggableMouseUpEvent);

        _unit.IsDraging = false;
    }

    
}
