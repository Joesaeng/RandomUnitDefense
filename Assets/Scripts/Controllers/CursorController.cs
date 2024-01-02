using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    enum CursorType
    {
        None,
    }

    CursorType _cursorType = CursorType.None;

    void Start()
    {
        
    }

    void Update()
    {
        UpdateMouseCursor();
    }

    void UpdateMouseCursor()
    {
        if (Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit, 100f, _mask))
        //{
        //    if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
        //    {
        //        if (_cursorType != CursorType.Attack)
        //        {
        //            Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
        //            _cursorType = CursorType.Attack;
        //        }
        //    }
        //    else
        //    {
        //        if (_cursorType != CursorType.Hand)
        //        {
        //            Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);
        //            _cursorType = CursorType.Hand;
        //        }
        //    }
        //}
    }
}
