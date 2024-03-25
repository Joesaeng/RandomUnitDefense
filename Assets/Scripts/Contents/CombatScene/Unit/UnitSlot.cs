using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSlot : MonoBehaviour
{
    [SerializeField]
    public int slotIndex;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Unit unit = collision.GetComponentInParent<Unit>();
        if(unit != null && unit.IsDraging == true)
        {
            spriteRenderer.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Unit unit = collision.GetComponentInParent<Unit>();
        if (unit != null && unit.IsDraging == true)
        {
            spriteRenderer.enabled = false;
        }
    }

    public void SetAbleUpgradeImage()
    {
        spriteRenderer.sprite = Managers.Resource.Load<Sprite>("Art/SlotMoveImage/Upgrade");
        spriteRenderer.enabled = true;
    }

    public void SetBasicImage()
    {
        spriteRenderer.sprite = Managers.Resource.Load<Sprite>("Art/SlotMoveImage/Move");

    }
}
