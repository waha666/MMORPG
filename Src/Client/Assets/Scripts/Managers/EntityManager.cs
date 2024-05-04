using Entities;
using SkillBridge.Message;
using System.Collections.Generic;

namespace Managers
{
	interface IEntityNotify
	{
		void OnEntityRemove();
	}

	class EntiyManager : Singleton<EntiyManager> 
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

    }
}
