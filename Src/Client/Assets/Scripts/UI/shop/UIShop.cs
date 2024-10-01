using Common.Data;
using Managers;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIBase
{
	public Text title;
	public Text money;

	public GameObject ShopItem;
	private ShopDefine shop;
	public Transform[] itemRoot;

	// Use this for initialization
	void Start () {
        StartCoroutine(InitItems());
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    IEnumerator InitItems()
    {
        int i = 0;
        foreach (var kv in DataManager.Instance.ShopItems[shop.ID])
        {
            int rootIndex = i >= 10 ? 1 : 0;
            if (kv.Value.Status > 0)
            {
                GameObject go = Instantiate(ShopItem, itemRoot[rootIndex]);
                UIShopItem ui = go.GetComponent<UIShopItem>();
                ui.setShopItem(kv.Key, kv.Value, this);
            }
            i++;
        }
        yield return null;
    }

    public void SetShop(ShopDefine shop) 
    {
        this.shop = shop;
        this.title.text = shop.Name;
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }

    private UIShopItem selectedItem;
    public void SelectShopItem(UIShopItem item) 
    {
        if (this.selectedItem != null) 
        {
            this.selectedItem.Selected = false;
        }
        selectedItem = item;
    }

    public void OnClickBuy()
    {
        if (this.selectedItem == null)
        {
            MessageBox.Show("请选择要购买的道具", "购买提示");
            return;
        }
        if (ShopManager.Instance.BuyItem(this.shop.ID,this.selectedItem.ShopItemID)) 
        { }
    }
}
