using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSlot : MonoBehaviour
{
    [SerializeField]
    public int slotIndex;

    SpriteRenderer spriteRenderer;
    Color objColor;

    bool isMouseDown = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        objColor = spriteRenderer.color;
        spriteRenderer.color = Color.clear;

        Managers.Input.MouseAction -= OnMouseAction;
        Managers.Input.MouseAction += OnMouseAction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Unit>() != null &&
            collision.gameObject.GetComponent<Unit>().IsDraging == true)
        {
            spriteRenderer.color = objColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Unit>() != null &&
            collision.gameObject.GetComponent<Unit>().IsDraging == true)
        {
            spriteRenderer.color = Color.clear;
        }
    }

    private void OnMouseAction(Define.MouseEvent mouseEvent)
    {
        switch (mouseEvent)
        {
            case Define.MouseEvent.PointerUp:
                spriteRenderer.color = Color.clear;
                break;
        }
    }
}
