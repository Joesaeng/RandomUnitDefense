using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnSelect : MonoBehaviour
{
    private void OnMouseDown()
    {
        Managers.Game.UnSelectUnit();
    }
}
