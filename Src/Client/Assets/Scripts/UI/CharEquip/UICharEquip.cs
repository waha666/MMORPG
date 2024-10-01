using Managers;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharEquip : UIBase {

	public Text title;
    public Text money;

    public GameObject itemPrefab;
    public GameObject itemEquipedPrefab;

    public Transform itemListRoot;

    public List<Transform> slots;

    public Text characterName;
    public Text hpText;
    public Slider hpBar;
    public Text mpText;
    public Slider mpBar;


    void Start()
    {
        RefreshUI();
        EquipManager.Instance.OnEquipChanged += RefreshUI;
    }

    // Update is called once per frame
    private void OnDestroy() 
    {
        EquipManager.Instance.OnEquipChanged -= RefreshUI;
    }

    private void RefreshUI()
    {
        ClearAllEquipList();
        InitAllEquipItems();
        ClearEquipList();
        InitEquipedItems();
        InitCharacterInfo();
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }

    private void ClearAllEquipList()
    {
        foreach (var item in itemListRoot.GetComponentsInChildren<UIEquipItem>()) 
        {
            Destroy(item.gameObject);
        }
    }

    private void InitAllEquipItems()
    {
        foreach (var kv in ItemManager.Instance.Items) 
        {
            if (kv.Value.Define.Type == ItemType.Equip && kv.Value.Define.LimitClass == User.Instance.CurrentCharacter.Class) 
            {
                if(EquipManager.Instance.Contanins(kv.Key))
                    continue;
                GameObject go = Instantiate(itemPrefab, itemListRoot);
                UIEquipItem ui = go.GetComponent<UIEquipItem>();
                ui.SetEquipItem(kv.Key, kv.Value, this, false);
            }
        }
    }

    private void ClearEquipList()
    {
        foreach (var item in this.slots) 
        {
            if (item.childCount > 0)
                Destroy(item.GetChild(0).gameObject);
        }
    }

    private void InitEquipedItems()
    {
        for (int i = 0;i<(int)EquipSlot.SlotMax;i++) 
        {
            var item = EquipManager.Instance.Equips[i];
            {
                if (item != null)
                {
                    GameObject go = Instantiate(itemEquipedPrefab, slots[i]);
                    UIEquipItem ui = go.GetComponent<UIEquipItem>();
                    ui.SetEquipItem(i, item, this, true);
                }
            }
        }
    }

    private void InitCharacterInfo()
    {
        string name = User.Instance.CurrentCharacter.Name;
        name += "  Lv." + User.Instance.CurrentCharacter.Level;
        this.characterName.text = name;
    }


    public void DoEquip(Item item) 
    {
        EquipManager.Instance.EquipItem(item);
    }

    public void UnEquip(Item item)
    {
        EquipManager.Instance.UnEquipItem(item);
    }

    private UIEquipItem selectedItem;
    public void SelectEquipItem(UIEquipItem item)
    {
        if (selectedItem == item) return;
        if (this.selectedItem != null)
        {
            this.selectedItem.Selected = false;

        }
        this.selectedItem = item;
    }
}
