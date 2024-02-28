using UnityEngine;
using UnityEngine.Tilemaps;

namespace Celeste.Tilemaps
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Simple Animated Tile", order = CelesteMenuItemConstants.TILEMAPS_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TILEMAPS_MENU_ITEM + "Simple Animated Tile")]
    public class SimpleAnimatedTile : TileBase
    {
        public Sprite[] animatedSprites;
        public float speed = 1f;
        public Tile.ColliderType m_TileColliderType;
        public bool randomizeStartTime = false;

        /// <summary>
        /// Retrieves any tile rendering data from the scripted tile.
        /// </summary>
        /// <param name="position">Position of the Tile on the Tilemap.</param>
        /// <param name="tilemap">The Tilemap the tile is present on.</param>
        /// <param name="tileData">Data to render the tile.</param>
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.transform = Matrix4x4.identity;
            tileData.color = Color.white;
            
            if (animatedSprites != null && animatedSprites.Length > 0)
            {
                tileData.sprite = animatedSprites[0];
                tileData.colliderType = m_TileColliderType;
            }
        }

        /// <summary>
        /// Retrieves any tile animation data from the scripted tile.
        /// </summary>
        /// <param name="position">Position of the Tile on the Tilemap.</param>
        /// <param name="tilemap">The Tilemap the tile is present on.</param>
        /// <param name="tileAnimationData">Data to run an animation on the tile.</param>
        /// <returns>Whether the call was successful.</returns>
        public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
        {
            if (animatedSprites.Length > 0)
            {
                tileAnimationData.animatedSprites = animatedSprites;
                tileAnimationData.animationSpeed = speed;
                tileAnimationData.animationStartTime = randomizeStartTime ? Random.Range(0, animatedSprites.Length / speed) : 0;
                
                return true;
            }

            return false;
        }
    }
}
