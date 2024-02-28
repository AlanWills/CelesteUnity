using Celeste.Tools.Attributes.GUI;
using UnityEngine;

namespace Celeste.UI
{
    [CreateAssetMenu(fileName = nameof(CanvasLayer), order = CelesteMenuItemConstants.UI_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.UI_MENU_ITEM + "Canvas/Canvas Layer")]
    public class CanvasLayer : ScriptableObject
    {
        public int SortOrder
        {
            get { return sortOrder; }
            set
            {
                if (sortOrder != value)
                {
                    sortOrder = value;
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.SetDirty(this);
#endif
                }
            }
        }

        [SerializeField, ReadOnly] private int sortOrder = 0;
    }
}