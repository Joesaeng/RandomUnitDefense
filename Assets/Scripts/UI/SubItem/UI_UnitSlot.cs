using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UnitSlot : UI_LobbySceneSlot
{
    UI_LobbyScene _uI_LobbyScene;

    public UnitNames ID { get; private set; }

    enum Images
    {
        UnitImage
    }

    public void Set(UnitNames unitId,UI_LobbyScene uI_LobbyScene)
    {
        ID = unitId;
        _uI_LobbyScene = uI_LobbyScene;
        Image image = GetImage((int)Images.UnitImage);
        image.transform.localPosition = new Vector3(0f, 10f, 0f);
        image.rectTransform.sizeDelta = new Vector2(300f, 300f);
        image.sprite = Managers.Resource.Load<Sprite>($"Art/Units/{ID}");
        gameObject.AddUIEvent(ClickedUnitSlot);

    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
        base.Init();
        parentSlotsScroll = GameObject.FindWithTag("UnitSlotsScroll").GetComponent<ScrollRect>();
    }

    public void ClickedUnitSlot(PointerEventData data)
    {
        if(!forParent)
        {
            Managers.Sound.Play(Define.SFXNames.Click);
            _uI_LobbyScene.OnSelectUnitButtonClick(ID);
        }
    }
}
