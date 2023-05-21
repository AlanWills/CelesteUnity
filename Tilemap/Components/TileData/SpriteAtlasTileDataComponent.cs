using Celeste.Components;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

namespace Celeste.Tilemaps.Components
{
    [CreateAssetMenu(fileName = nameof(SpriteAtlasTileDataComponent), menuName = "Celeste/Tiles/Components/Sprite Atlas Tile Data Component")]
    public class SpriteAtlasTileDataComponent : TileComponent, ITileDataComponent
    {
        #region Save Data

        [Serializable]
        private class SaveData : ComponentData
        {
            public string spriteName;
        }

        #endregion

        #region Properties and Fields

        public SpriteAtlas SpriteAtlas => spriteAtlas;

        [SerializeField] private SpriteAtlas spriteAtlas;

        #endregion

        public override ComponentData CreateData()
        {
            return new SaveData();
        }

        public void GetTileData(Instance instance, Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            SaveData saveData = instance.data as SaveData;
            tileData.sprite = spriteAtlas.GetSprite(saveData.spriteName);
            tileData.transform = Matrix4x4.identity;
            tileData.color = Color.white;
            tileData.colliderType = Tile.ColliderType.Sprite;
        }
    }
}
