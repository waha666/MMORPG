using Common.Data;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

    public int ItemID;
    public int Count;
    public ItemDefine Define;
    public EquipDefine EquipInfo;
    // Use this for initialization
    public Item(NItemInfo item) :
        this(item.Id, item.Count)
    { 
    }
    
    public Item(int id,int count)
    {

        this.ItemID = id;
        this.Count = count;
        DataManager.Instance.Items.TryGetValue(this.ItemID, out this.Define);
        DataManager.Instance.Equips.TryGetValue(this.ItemID, out this.EquipInfo);
    }

    public override string ToString()
    {
        return string.Format("ID:{0},Count{1}", this.ItemID, this.Count);
    }
}
