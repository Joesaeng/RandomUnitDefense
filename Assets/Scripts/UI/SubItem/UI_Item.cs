using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_Item : UI_Base
{
    enum GameObjects
    {
        ImageItemInfo
    }

    enum TMPros
    {
        TextItemLevel,
        TextItemInfo,
    }

    void Start()
    {

    }

    private void Awake()
    {
        Init();
    }

    InGameItemData _itemdata;
    public Action OnClickedItemButton;

    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(TMPros));
        Bind<GameObject>(typeof(GameObjects));

        gameObject.AddUIEvent(ClickedItem);
    }

    public void SetInfo(InGameItemData itemdata)
    {
        GameObject infoObject = Get<GameObject>((int)GameObjects.ImageItemInfo);
        infoObject.SetActive(true);
        _itemdata = itemdata;
        string itemImagePath = $"{_itemdata.itemName}_{_itemdata.itemLevel}";
        gameObject.GetComponent<Image>().sprite =
            Managers.Resource.Load<Sprite>($"Art/InGameItems/{itemImagePath}");

        GetTMPro((int)TMPros.TextItemLevel).text = $"{(ItemLevelString)_itemdata.itemLevel}";
        GetTMPro((int)TMPros.TextItemLevel).color = ConstantData.ItemColors[_itemdata.itemLevel];

        ItemName itemName = Util.Parse<ItemName>(_itemdata.itemName);
        string itemInfoValue = "";
        switch (itemName)
        {
            case ItemName.increaseDamageItem:
                itemInfoValue = $"{(int)(_itemdata.increaseDamage * 100)}%";
                break;
            case ItemName.attackRateItem:
                itemInfoValue = $"{(int)(_itemdata.decreaseAttackRate * 100)}%";
                break;
            case ItemName.attackRangeItem:
                itemInfoValue = $"{(_itemdata.increaseAttackRange)}";
                break;
            case ItemName.AOEAreaItem:
                itemInfoValue = $"{(int)(_itemdata.increaseAOEArea * 100)}%";
                break;
            case ItemName.addedDamageItem:
                itemInfoValue = $"{_itemdata.addedDamage}";
                break;
        }
        GetTMPro((int)TMPros.TextItemInfo).text = $"{Language.GetItemInfo(itemName)} : {itemInfoValue}";
        infoObject.SetActive(false);
    }

    enum ItemLevelString
    {
        Common = 1,
        UnCommon,
        Rare,
        Unique,
        Legend
    }

    public void ClickedItem(PointerEventData data)
    {
        Managers.Sound.Play(Define.SFXNames.Click);
        GameObject infoObject = Get<GameObject>((int)GameObjects.ImageItemInfo);
        if (infoObject.activeSelf)
        {
            infoObject.SetActive(false);
            return;
        }
        Util.CheckTheEventAndCall(OnClickedItemButton);
        infoObject.SetActive(true);
    }

    public void DeactiveInfoObject()
    {
        GameObject infoObject = Get<GameObject>((int)GameObjects.ImageItemInfo);
        infoObject.SetActive(false);
    }

    public override void OnChangeLanguage()
    {
        
    }
}
