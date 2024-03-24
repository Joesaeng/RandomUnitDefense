using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        if(spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (spriteRenderer.enabled == false)
            Managers.Resource.Destroy(gameObject);
    }
}
