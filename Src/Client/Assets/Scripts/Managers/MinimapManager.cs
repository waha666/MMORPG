 using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    class MinimapManager : Singleton<MinimapManager>
    {
        public UIMinimap minimap;
        private Collider minimapBoundingBox;
        public Collider MinimapBoundingBox
        {
            get { return this.minimapBoundingBox;}
            
        }
        public Sprite LoadCurrentMinimap()
        {
            return Resloader.Load<Sprite>("UI/Minimap/"+ User.Instance.CurrentMapData.MiniResource);
        }

        public void UpdateMinimap(Collider BoundingBox) 
        {
            this.minimapBoundingBox = BoundingBox;
            if (this.minimap != null)
                this.minimap.UpdateMap();
        }
    }
}
