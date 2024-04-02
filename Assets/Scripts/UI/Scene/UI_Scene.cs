using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);
    }

    protected void SetCanvasRenderModeCamera()
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(this.gameObject);
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;

        canvas.overrideSorting = true;

        canvas.sortingOrder = ConstantData.SceneUISortOrder;
    }

    public override void OnChangeLanguage()
    {
        
    }
}
