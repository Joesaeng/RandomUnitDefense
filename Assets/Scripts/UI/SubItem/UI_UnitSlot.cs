using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UnitSlot : UI_Base, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    NestedScrollManager NM;
    ScrollRect baseScrollRect;
    ScrollRect UnitSlotsScroll;

    UI_LobbyScene _uI_LobbyScene;

    bool forParent;

    public UnitNames ID { get; private set; }

    enum Images
    {
        UnitImage
    }

    public void Set(UnitNames unitId,UI_LobbyScene uI_LobbyScene)
    {
        ID = unitId;
        _uI_LobbyScene = uI_LobbyScene;
        string imagePath = $"{ID}_1_Idle";
        Image image = GetImage((int)Images.UnitImage);
        image.transform.localPosition = new Vector3(0f, 10f, 0f);
        image.rectTransform.sizeDelta = new Vector2(300f, 300f);
        image.sprite = Managers.Resource.Load<Sprite>($"Art/Units/{ID}");
        gameObject.AddUIEvent(ClickedUnitSlot);

    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
        NM = GameObject.FindWithTag("NestedScrollManager").GetComponent<NestedScrollManager>();
        baseScrollRect = GameObject.FindWithTag("NestedScrollManager").GetComponent<ScrollRect>();
        UnitSlotsScroll = GameObject.FindWithTag("UnitSlotsScroll").GetComponent<ScrollRect>();
    }

    public void ClickedUnitSlot(PointerEventData data)
    {
        if(!forParent)
        {
            Managers.Sound.Play(Define.SFXNames.Click);
            _uI_LobbyScene.OnSelectUnitButtonClick(ID);
        }
    }

    // 스크롤의 드래그를 위함
    public void OnBeginDrag(PointerEventData eventData)
    {
        forParent = Mathf.Abs(eventData.delta.x) > 1f && Mathf.Abs(eventData.delta.y) > 1f;
        if(forParent)
            forParent = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);

        if (forParent)
        {
            NM.OnBeginDrag(eventData);
            baseScrollRect.OnBeginDrag(eventData);
        }
        else
            UnitSlotsScroll.OnBeginDrag(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            NM.OnDrag(eventData);
            baseScrollRect.OnDrag(eventData);
        }
        else
            UnitSlotsScroll.OnDrag(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            NM.OnEndDrag(eventData);
            baseScrollRect.OnEndDrag(eventData);
        }
        else
            UnitSlotsScroll.OnEndDrag(eventData);
    }

    private void Start()
    {
        
    }
    private void Awake()
    {
        Init();
    }

    public override void OnChangeLanguage()
    {
        
    }


}
