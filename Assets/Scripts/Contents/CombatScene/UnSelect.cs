using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnSelect : MonoBehaviour
{
    private void OnMouseDown()
    {
        // if (EventSystem.current.IsPointerOverGameObject())
        //     return;
        if (Managers.Input.IsPointerOverUIObject())
            return;
        Managers.Game.UnSelectUnit();
    }
}
