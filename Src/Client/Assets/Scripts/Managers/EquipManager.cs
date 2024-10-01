using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class EquipManager : Singleton<EquipManager>
    {
        public delegate void OnEquipChangeHadler();
        public event OnEquipChangeHadler OnEquipChanged;
        public Item[] Equips = new Item[(int)EquipSlot.SlotMax];
        byte[] Data;

        unsafe public void Init(byte[] data) 
        {
            this.Data = data;
            this.ParseEquipData();
        }

        unsafe void ParseEquipData() 
        {
            fixed (byte* pt = this.Data)
            {
                for (int i = 0; i < this.Equips.Length; i++)
                {
                    int itemId = *(int*)(pt + i * sizeof(int));
                    if (itemId > 0)
                        Equips[i] = ItemManager.Instance.Items[itemId];
                    else
                        Equips[i] = null;
                }
            }
        }


        unsafe public byte[] GetEquipData()
        {
            fixed (byte* pt = this.Data)
            {
                for (int i = 0; i < this.Equips.Length; i++)
                {
                    int* itemId = (int*)(pt + i * sizeof(int));
                    if (this.Equips[i] == null)
                        *itemId = 0;
                    else
                        *itemId = this.Equips[i].ItemID;
                }
            }
            return this.Data;
        }

        public Item GetEquip(EquipSlot slot) 
        {
            return this.Equips[(int)slot];
        }


        public void EquipItem(Item equip) 
        {
            ItemService.Instance.SendEquipItem(equip, true);
        }

        public void UnEquipItem(Item equip)
        {
            ItemService.Instance.SendEquipItem(equip, false);
        }


        internal void OnEquipItem(Item pendingEquip)
        {
            if (this.Equips[(int)pendingEquip.EquipInfo.Slot] != null && this.Equips[(int)pendingEquip.EquipInfo.Slot].ItemID == pendingEquip.ItemID) 
            {
                return;
            }
            this.Equips[(int)pendingEquip.EquipInfo.Slot] = ItemManager.Instance.Items[pendingEquip.ItemID];
            if (OnEquipChanged != null)
                OnEquipChanged();
        }

        internal void OnUnEquipItem(EquipSlot slot)
        {
            if (this.Equips[(int)slot] != null)
            {
                this.Equips[(int)slot] = null;
                if (OnEquipChanged != null)
                    OnEquipChanged();
            }
            
        }

        internal bool Contanins(int itemId)
        {
            for (int i = 0; i < this.Equips.Length; i++) 
            {
                if (this.Equips[i] != null && this.Equips[i].ItemID == itemId)
                    return true;
            }
            return false;
        }
    }
}
