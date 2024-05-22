
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
                if (kv.Value.Count <= kv.Value.Define.StackLimt)
                {
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)kv.Value.Count;
                }
                else
                {
                    int count = kv.Value.Count;
                    while (count > kv.Value.Define.StackLimt)
                    {
                        this.Items[i].ItemId = (ushort)kv.Key;
                        this.Items[i].Count = (ushort)kv.Value.Define.StackLimt;
                        i++;
                        count -= kv.Value.Define.StackLimt;
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
    }
}
