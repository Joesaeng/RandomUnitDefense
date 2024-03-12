using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSlots : MonoBehaviour
{
    SpriteRenderer[] unitSlotsSpriteRenderer;
    void Start()
    {
        unitSlotsSpriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        Managers.Input.MouseAction.AddEvent(OnMouseAction);
    }
    private void OnMouseAction(Define.MouseEvent mouseEvent)
    {
        switch (mouseEvent)
        {
            case Define.MouseEvent.PointerUp:
                foreach (SpriteRenderer spriteRenderer in unitSlotsSpriteRenderer)
                {
                    spriteRenderer.enabled = false;
                }
                break;
        }
    }
}
