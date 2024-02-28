using Celeste.Objects;
using System;
using UnityEngine;

namespace Celeste.UI
{
    [CreateAssetMenu(fileName = nameof(CanvasLayers), order = CelesteMenuItemConstants.UI_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.UI_MENU_ITEM + "Canvas/Canvas Layers")]
    public class CanvasLayers : ListScriptableObject<CanvasLayer>
    {
        #region Properties and Fields

        public int SortOrder
        {
            get { return sortOrder; }
            set
            {
                if (sortOrder != value)
                {
                    sortOrder = value;

                    Synchronize();
                }
            }
        }

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