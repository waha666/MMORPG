using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class SpawnManager
    {
        private List<Spawner> Rules = new List<Spawner>();

        private Map m_map;

        public void Init(Map map) 
        {
            this.m_map = map;
            if (DataManager.Instance.SpawnRules.ContainsKey(map.Define.ID)) 
            {
                foreach (var define in DataManager.Instance.SpawnRules[map.Define.ID].Values) 
                {
                    this.Rules.Add(new Spawner(define, this.m_map));
                }
            }
        }

        public void Update() 
        {
            if (Rules.Count == 0) return;
            for (int i = 0; i < Rules.Count; i++) 
            {
                this.Rules[i].Update();
            }
        }

    }
}
