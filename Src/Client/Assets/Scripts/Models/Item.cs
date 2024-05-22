using Common.Data;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

    public int ItemID;
    public int Count;

    public ItemDefine Define;
    // Use this for initialization
    public Item(NItemInfo item)
    {

        this.ItemID = (short)item.Id;
        this.Count = (short)item.Count;
        this.Define = DataManager.Instance.Items[this.ItemID];
    }

    public override string ToString()
    {
        return string.Format("ID:{0},Count{1}", this.ItemID, this.Count);
    }
}
