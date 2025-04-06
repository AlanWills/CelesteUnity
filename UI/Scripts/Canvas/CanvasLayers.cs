using Celeste.Objects;
using System;
using UnityEngine;

namespace Celeste.UI
{
    [CreateAssetMenu(fileName = nameof(CanvasLayers), order = CelesteMenuItemConstants.UI_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.UI_MENU_ITEM + "Canvas/Canvas Layers")]
    public class CanvasLayers : ListScriptableObject<CanvasLayer>, IAutomaticImportSettings
    {
        #region Properties and Fields

        public int SortOrder
        {
            get => sortOrder;
            set
            {
                if (sortOrder != value)
                {
                    sortOrder = value;

                    Synchronize();
                }
            }
        }

        public AutomaticImportBehaviour ImportBehaviour => automaticImportBehaviour;

        [SerializeField] private AutomaticImportBehaviour automaticImportBehaviour = AutomaticImportBehaviour.ImportAssetsInCatalogueDirectoryAndSubDirectories;
        [SerializeField] private int sortOrder = 0;

        #endregion

        public void Synchronize()
        {
            for (int i = 0; i < NumItems; ++i)
            {
                CanvasLayer canvasLayer = GetItem(i);
                if (canvasLayer != null)
                {
                    canvasLayer.SortOrder = i + sortOrder;
                }
            }

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }
}