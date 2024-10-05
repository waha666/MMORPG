using Common;
using Common.Data;
using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class Spawner
    {
        public SpawnRuleDefine Define { get; set; }

        private Map m_map;

        /// <summary>
        /// 刷新时间
        /// </summary>
        private float m_spawnTime = 0;

        /// <summary>
        /// 消失时间
        /// </summary>
        private float unspawnTime = 0;

        private bool spawned = false;

        private SpawnPointDefine spawnPoint = null;

        public Spawner(SpawnRuleDefine define, Map map) 
        {
            this.Define = define;
            this.m_map = map;

            if (DataManager.Instance.SpawnPoints.ContainsKey(this.m_map.ID)) 
            {
                if (DataManager.Instance.SpawnPoints[this.m_map.ID].ContainsKey(this.Define.SpawnPoint))
                {
                    spawnPoint = DataManager.Instance.SpawnPoints[this.m_map.ID][this.Define.SpawnPoint];
                }
                else
                {
                    Log.ErrorFormat("SpawnRule[{0}], SpawnPoint[{1}], not existed", this.Define.ID, this.Define.SpawnPoint);
                }
            }
        }

        public void Update() 
        {
            if (this.CanSpawn()) 
            {
                this.Spawn();
            }
        }

        bool CanSpawn() 
        {
            if (this.spawned) 
            {
                return false;
            }
            if (this.unspawnTime + this.Define.SpawnPeriod > Time.time) 
            {
                return false;
            }
            return true;
        }

        public void Spawn() 
        {
            this.spawned = true;
            Log.InfoFormat("Map[{0}],Spawn[{1}],Mon{2},Lv{3} At Piont:{4}",this.Define.MapID,this.Define.ID,this.Define.SpawnMonID, this.Define.SpawnLevel, this.spawnPoint.Position);
            this.m_map.monsterManager.Create(this.Define.SpawnMonID, this.Define.SpawnLevel, this.spawnPoint.Position, this.spawnPoint.Direction);
        }

    }
}
