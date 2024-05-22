using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager> {

    public Dictionary<int, Item> Items = new Dictionary<int, Item>();
    internal void Init(List<NItemInfo> items) 
    {
        this.Items.Clear();
        foreach (var info in items)
        {
            Item item = new Item(info);
            Items.Add(item.ItemID, item);

            Debug.LogFormat("ItemManager:Init[{0}]", item);
        }
    }
    public Item GetItem(int itemId)
    {
        Item item = null;
        this.Items.TryGetValue(itemId, out item);
        return item;
    }
}
