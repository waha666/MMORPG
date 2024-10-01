﻿
using Models;
using SkillBridge.Message;

namespace Managers
{

	class BagManager : Singleton<BagManager>
    {
        public int Unlocked;
        public BagItem[] Items;
        NBagInfo Info;
        unsafe public void Init(NBagInfo info)
        {
            this.Info = info;
            this.Unlocked = info.Unlocked;
            Items = new BagItem[this.Unlocked];
            if (info.Items != null && info.Items.Length >= this.Unlocked)
            {
                Analye(info.Items);
            }
            else
            {
                info.Items = new byte[sizeof(BagItem) * this.Unlocked];
                Reset();
            }
        }

        private void Reset()
        {
            int i = 0;
            foreach (var kv in ItemManager.Instance.Items)
            {
                if ( i>= this.Items.Length) break;
                if (kv.Value.Count <= kv.Value.Define.StackLimit)
                {
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)kv.Value.Count;
                }
                else
                {
                    int count = kv.Value.Count;
                    while (count > kv.Value.Define.StackLimit)
                    {
                        if (i >= this.Items.Length) break;
                        this.Items[i].ItemId = (ushort)kv.Key;
                        this.Items[i].Count = (ushort)kv.Value.Define.StackLimit;
                        i++;
                        count -= kv.Value.Define.StackLimit;
                    }
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)count;
                }
                i++;
            }
        }

        unsafe void Analye(byte[] data)
        {
            fixed (byte* pt = data)
            {
                for (int i = 0; i < this.Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    item[i] = *item;
                }
            }
        }

        unsafe public NBagInfo GetBagInfo()
        {
            fixed (byte* pt = Info.Items)
            {
                for (int i = 0; i < this.Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    *item = Items[i];
                }
            }
            return this.Info;
        }

        public void AddItem(int itemId, int count) 
        {
            ushort addCount = (ushort)count;
            for (int i = 0; i < this.Items.Length; i++) 
            {
                if (this.Items[i].ItemId == itemId) 
                {
                    ushort canAdd = (ushort)(DataManager.Instance.Items[itemId].StackLimit - this.Items[i].Count);
                    if (canAdd >= count)
                    {
                        this.Items[i].Count += addCount;
                        addCount = 0;
                        break;
                    }
                    else 
                    {
                        this.Items[i].Count += canAdd;
                        addCount -= canAdd;
                    }
                }
            }
            if (addCount > 0) 
            {
                for (int i = 0; i < this.Items.Length; i++) 
                {
                    if (this.Items[i].ItemId == 0) 
                    {
                        this.Items[i].ItemId = (ushort)itemId;
                        this.Items[i].Count = addCount;
                        break;
                    }
                }
            } 
        }

        public void RemoveItem(int itemId, int count) { }
    }
}
