using Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;

namespace Managers
{
	interface IEntityNotify
	{
		void OnEntityRemove();
        void OnEntityChanged(Entity entity);
        void OnEntityEvent(EntityEvent @entityEvent);
    }

	class EntityManager : Singleton<EntityManager> 
	{
		Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        Dictionary<int, IEntityNotify> notifiers = new Dictionary<int, IEntityNotify>();

		public void RegisterEntityChangeNotify(int entityId, IEntityNotify notify) 
		{
			this.notifiers[entityId] = notify;
        }

		public void AddEntity(Entity entity)
		{
			this.entities[entity.entityId] = entity;

        }

        public void ReomveEntity(NEntity entity)
        {
			this.entities.Remove(entity.Id);
			if (notifiers.ContainsKey(entity.Id)) 
			{
				notifiers[entity.Id].OnEntityRemove();
				notifiers.Remove(entity.Id);

            }
        }

        internal void OnEntitySync(NEntitySync nEntity)
        {
			Entity entity = null;
			entities.TryGetValue(nEntity.Id, out entity);
			if (entity != null) 
			{
				if (nEntity.Entity != null) 
				{
					entity.EntityData = nEntity.Entity;
                }
				if (notifiers.ContainsKey(nEntity.Id)) 
				{
					notifiers[entity.entityId].OnEntityChanged(entity);
                    notifiers[entity.entityId].OnEntityEvent(nEntity.Event);
                }
			}
        }
    }
}
