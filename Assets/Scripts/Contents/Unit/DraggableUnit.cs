using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUnit : MonoBehaviour
{
    public Action OnMouseUpEvent;
    Vector3 mousePos;
    Unit unit;

    private void Start()
    {
        unit = transform.GetComponent<Unit>();
    }

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    public void OnMouseDown()
    {
        unit.IsDraging = true;
        mousePos = Input.mousePosition - GetMousePos();
        //TEMP
        Debug.Log($"ID : {unit.Stat.id}");
    }

    public void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePos);
    }

    public void OnMouseUp()
    {
        if (OnMouseUpEvent != null)
            OnMouseUpEvent.Invoke();

        unit.IsDraging = false;
    }
}
