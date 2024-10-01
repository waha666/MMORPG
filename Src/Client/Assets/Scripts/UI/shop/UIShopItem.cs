using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour, ISelectHandler {

    public Image icon;
    public Text title;
    public Text price;
    public Text count;
    public Text vocation;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    private bool selected;
    public bool Selected 
    {
        get { return selected; }
        set 
        {
            selected = value;
            this.background.overrideSprite = selected ? selectedBg : normalBg;
        }
    }
    public int ShopItemID { get; set; }
    private UIShop shop;

    private ItemDefine item;
    private ShopItemDefine shopItem;

    internal void setShopItem(int id, ShopItemDefine shopItem, UIShop owner)
    {
        this.shop = owner;
        this.ShopItemID = id;
        this.shopItem = shopItem;
        this.item = DataManager.Instance.Items[this.shopItem.ItemID];

        this.title.text = this.item.Name;
        this.count.text = this.shopItem.Count.ToString();
        this.price.text = this.shopItem.Price.ToString();
        if (this.item.LimitClass > 0)
            this.vocation.text = this.item.LimitClass.ToString();
        else
            this.vocation.text = "";


        this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Icon);
    }

    public void OnSelect(BaseEventData eventData)
    {
        this.Selected = true;
        this.shop.SelectShopItem(this);
    }
}
