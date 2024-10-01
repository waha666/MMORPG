using Managers;
using Models;
using Services;
using SkillBridge.Message;
using System;
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
        StatusService.Instance.RegisterStatusEvent(StatusType.Item, OnItemNotify);
    }

    private bool OnItemNotify(NStatus status)
    {
        if (status.Action == StatusAction.Add)
        {
            this.AddItem(status.Id, status.Value);
        }
        else if (status.Action == StatusAction.Delete) 
        {
            this.RemoveItem(status.Id, status.Value);
        }
        return true;
    }

    private void AddItem(int id, int count)
    {
        Item item = null;
        if (this.Items.TryGetValue(id, out item))
        {
            item.Count += count;
        }
        else 
        {
            item = new Item(id, count);
            this.Items.Add(id, item);
        }
        BagManager.Instance.AddItem(id, count);
    }

    private void RemoveItem(int id, int count)
    {
        if (!this.Items.ContainsKey(id)) 
        {
            return;
        }
        Item item = this.Items[id];
        if (item.Count < count) return;
        item.Count -= count;
        BagManager.Instance.RemoveItem(id, count);
    }

    public Item GetItem(int itemId)
    {
        Item item = null;
        this.Items.TryGetValue(itemId, out item);
        return item;
    }
}
