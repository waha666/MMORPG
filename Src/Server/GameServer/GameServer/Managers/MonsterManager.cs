using GameServer.Entities;
using GameServer.Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class MonsterManager
    {
        private Map m_map;

        public Dictionary<int, Monster> Monsters = new Dictionary<int, Monster>();

        public void Init(Map map) 
        {
            this.m_map = map;
        }

        internal Monster Create(int spawnMonId, int spawnLevel, NVector3 position, NVector3 direction) 
        {
            Monster monster = new Monster(spawnMonId, spawnLevel, position, direction);
            EntityManager.Instance.AddEntity(this.m_map.ID, monster);
            monster.Info.Id = monster.entityId;
            monster.Info.mapId = this.m_map.ID;
            Monsters[monster.entityId] = monster;

            this.m_map.MonsterEnter(monster);
            return monster;
        }

    }
}
