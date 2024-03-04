using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUnit : MonoBehaviour
{
    public Action OnMouseUpEvent;
    public Action OnMouseClickEvent;
    public Action OnMouseDragEvent;
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
        _mousePos = Input.mousePosition - GetMousePos();
    }

    public void OnMouseDrag()
    {
        if (_pressed)
        {
            if (Time.time > _pressedTime + 0.2f)
                Managers.Game.UnSelectUnit();
        }
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - _mousePos);
    }

    public void OnMouseUp()
    {
        if (_pressed)
        {
            if (Time.time < _pressedTime + 0.2f)
                OnMouseClickEvent.Invoke();
        }
        _pressed = false;
        _pressedTime = 0;

        if (OnMouseUpEvent != null)
            OnMouseUpEvent.Invoke();

        _unit.IsDraging = false;
    }
}
