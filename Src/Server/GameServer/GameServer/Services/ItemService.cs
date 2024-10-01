using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class ItemService : Singleton<ItemService>
    {
        public ItemService() 
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemBuyRequest>(this.OnItemBuy);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemEquipRequest>(this.OnItemEquip);
        }

        public void Init() 
        {
        
        }

        void OnItemBuy(NetConnection<NetSession> sender, ItemBuyRequest response) 
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnItemBuy:character:{0};shop:{1};shopItem:{2}", character.Id, response.shopId, response.shopItemId);
            var result = ShopManager.Instance.BuyItem(sender, response.shopId, response.shopItemId);
            sender.Session.Response.itemBuy = new ItemBuyResponse();
            sender.Session.Response.itemBuy.Reult = result;
            sender.SendResponse();
            
        }

        private void OnItemEquip(NetConnection<NetSession> sender, ItemEquipRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnItemEquip:character:{0};Slot:{1};Item:{2};Equip:{3}", character.Id, message.Slot, message.Itemid, message.isEquip);
            var result = EquipManager.Instance.EquipItem(sender, message.Slot, message.Itemid, message.isEquip);
            sender.Session.Response.itemEquip = new ItemEquipResponse();
            sender.Session.Response.itemEquip.Reult = result;
            sender.SendResponse();
        }

    }
}
