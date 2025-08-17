using Celeste.Components;
using Celeste.Objects;
using Celeste.Tilemaps.Components;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Celeste.Tilemaps.Tiles
{
    [CreateAssetMenu(fileName = nameof(TileWithComponents), order = CelesteMenuItemConstants.TILEMAPS_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TILEMAPS_MENU_ITEM + "Tile With Components")]
    public class TileWithComponents : TileBase, IComponentContainerUsingTemplates<TileComponent>, IEditorInitializable
    {
        #region Properties and Fields

        public int NumComponents => components.NumComponents;

        [HideInInspector, SerializeField] private TileComponents components;

        [NonSerialized] private ComponentContainerRuntime<TileComponent> componentRuntimes = new ComponentContainerRuntime<TileComponent>();
        [NonSerialized] private bool hasCheckedForTileDataComponent = false;
        [NonSerialized] private bool hasCheckedForTileAnimationDataComponent = false;
        [NonSerialized] private InterfaceHandle<ITileDataComponent> tileDataComponent = InterfaceHandle<ITileDataComponent>.NULL;
        [NonSerialized] private InterfaceHandle<ITileAnimationDataComponent> tileAnimationDataComponent = InterfaceHandle<ITileAnimationDataComponent>.NULL;


        #endregion

        public void Editor_Initialize()
        {
#if UNITY_EDITOR
            if (components == null)
            {
                components = CreateInstance<TileComponents>();
                components.name = $"{name}_Components";
                UnityEditor.AssetDatabase.AddObjectToAsset(components, this);
            }
#endif
        }

        #region TileBase Overrides

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            componentRuntimes.InitializeComponents(components);

            return base.StartUp(position, tilemap, go);
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            if (!hasCheckedForTileDataComponent)
            {
                componentRuntimes.TryFindComponent(out tileDataComponent);
                hasCheckedForTileDataComponent = true;
            }

            if (tileDataComponent.IsValid)
            {
                tileDataComponent.iFace.GetTileData(tileDataComponent.instance, position, tilemap, ref tileData);
            }

            base.GetTileData(position, tilemap, ref tileData);
        }

        public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
        {
            if (!hasCheckedForTileAnimationDataComponent)
            {
                componentRuntimes.TryFindComponent(out tileAnimationDataComponent);
                hasCheckedForTileAnimationDataComponent = true;
            }

            if (tileAnimationDataComponent.IsValid)
            {
                return tileAnimationDataComponent.iFace.GetTileAnimationData(
                    tileAnimationDataComponent.instance, 
                    position, 
                    tilemap, 
                    ref tileAnimationData);
            }

            return base.GetTileAnimationData(position, tilemap, ref tileAnimationData);
        }

        #endregion

        public void SetComponentData(int index, ComponentData componentData)
        {
            components.SetComponentData(index, componentData);
        }

        public ComponentData GetComponentData(int index)
        {
            return components.GetComponentData(index);
        }

        public TileComponent GetComponent(int index)
        {
            return components.GetComponent(index);
        }

        public bool HasComponent<K>() where K : TileComponent
        {
            return components.HasComponent<K>();
        }

        public K FindComponent<K>() where K : TileComponent
        {
            return components.FindComponent<K>();
        }

        public void RemoveComponent(int componentIndex)
        {
            components.RemoveComponent(componentIndex);
        }
    }
}
