using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Celeste.Tilemaps
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Rotating Tile", order = CelesteMenuItemConstants.TILEMAPS_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TILEMAPS_MENU_ITEM + "Rotating Tile")]
    public class RotatingTile : TileBase
    {
        protected class RotatingTileInstance
        {
            public float currentRotation = 0;
        }

        public Sprite sprite;
        public float degreesPerSecond = 360;
        public Tile.ColliderType tileColliderType;
        public bool loop = true;

        private Dictionary<Vector3Int, RotatingTileInstance> instances = new Dictionary<Vector3Int, RotatingTileInstance>();

        protected RotatingTileInstance AddInstance(Vector3Int position)
        {
            if (instances.TryGetValue(position, out RotatingTileInstance instance))
            {
                instance.currentRotation = 0;
            }
            else
            {
                instance = CreateInstance();
                instances.Add(position, instance);
            }

            return instance;
        }

        protected RotatingTileInstance GetInstance(Vector3Int position)
        {
            if (instances.TryGetValue(position, out RotatingTileInstance instance))
            {
                return instance;
            }

            return null;
        }

        protected virtual RotatingTileInstance CreateInstance()
        {
            return new RotatingTileInstance();
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.transform = Matrix4x4.identity;
            tileData.color = Color.white;
            tileData.sprite = sprite;
            tileData.colliderType = tileColliderType;

            if (Application.isEditor && !Application.isPlaying)
            {
                return;
            }

            RotatingTileInstance instance;
            if (!instances.TryGetValue(position, out instance))
            {
                instance = AddInstance(position);
            }

            instance.currentRotation -= degreesPerSecond * Time.deltaTime;
            instance.currentRotation %= 360;

            Quaternion q = Quaternion.AngleAxis(instance.currentRotation, new Vector3(0, 0, 1));
            tileData.transform = Matrix4x4.Rotate(q);
            tileData.flags = TileFlags.LockTransform;
        }
    }
}