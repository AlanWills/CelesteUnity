using Celeste.Objects;
using UnityEngine;

namespace Celeste.Inventory
{
    [CreateAssetMenu(fileName = nameof(InventoryItem), menuName = CelesteMenuItemConstants.INVENTORY_MENU_ITEM + "Inventory Item", order = CelesteMenuItemConstants.INVENTORY_MENU_ITEM_PRIORITY)]
    public class InventoryItem : ScriptableObject, IIntGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get { return guid; }
            set
            {
                guid = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public string DisplayName
        {
            get { return displayName; }
        }

        public Sprite SpriteNoBackground
        {
            get { return spriteNoBackground; }
        }

        public Sprite SpriteWithBackground
        {
            get { return spriteWithBackground; }
        }

        [SerializeField] private int guid;
        [SerializeField] private string displayName;
        [SerializeField] private Sprite spriteNoBackground;
        [SerializeField] private Sprite spriteWithBackground;

        #endregion
    }
}
