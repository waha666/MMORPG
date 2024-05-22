using System;
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using Models;
using UnityEngine;

namespace Managers 
{
    class NpcManager : Singleton<NpcManager>
    {
        public delegate bool NpcActionHandler(NpcDefine npc);
        Dictionary<NpcFunction, NpcActionHandler> eventMap = new Dictionary<NpcFunction, NpcActionHandler>();
        public BagItem[] Items;

        public void RegisterNpcEvent(NpcFunction function, NpcActionHandler action)
        {
            if (!this.eventMap.ContainsKey(function))
            {
                eventMap[function] = action;
            }
            else 
            {
                eventMap[function] += action;
            }
        }

        public NpcDefine GetNpcDefine(int npcID) 
        {
            return DataManager.Instance.Npcs[npcID];
        }

        public bool Interactive(int npcID) 
        {
            if (DataManager.Instance.Npcs.ContainsKey(npcID)) 
            {
                var npc = DataManager.Instance.Npcs[npcID];
                return Interactive(npc);
            }
            return false;
        }

        public bool Interactive(NpcDefine npc)
        {
            if (npc.Type == NpcType.Task)
            {
                return DoTaskInteractive(npc);
            }
            else if(npc.Type == NpcType.Functional)
            {
                return DoFunctionInteractive(npc);
            }
            return false;
        }

   
        private bool DoTaskInteractive(NpcDefine npc)
        {
            MessageBox.Show("点击了NPC：" + npc.Name, "NPC对话");
            return true;
        }

        private bool DoFunctionInteractive(NpcDefine npc)
        {
            if (npc.Type != NpcType.Functional)
                return false;
            if (!eventMap.ContainsKey(npc.Function))
                return false;
            return eventMap[npc.Function](npc);
        }

    }
}

