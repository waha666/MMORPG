﻿using Common.Data;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
	class ShopManager : Singleton<ShopManager>
	{

		public void Init() 
		{
			NpcManager.Instance.RegisterNpcEvent(NpcFunction.lnvokeShop, OnOpenShop);
		}

        public bool OnOpenShop(NpcDefine npc)
        {
			this.ShowShop(npc.Param);
			return true;
        }

        private void ShowShop(int shopId)
        {
            ShopDefine shop;
            if (DataManager.Instance.Shops.TryGetValue(shopId, out shop))
            {
                UIShop uiShop = UIManager.Instance.Show<UIShop>();
                if (uiShop != null) 
                {
                    uiShop.SetShop(shop);
                }
            }
        }

        public bool BuyItem(int shopId, int shopItemId) 
        {
            ItemService.Instance.SendBuyItem(shopId, shopItemId);
            return true;
        }
    }
}