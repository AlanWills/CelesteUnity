using Celeste.Objects;
using UnityEngine;

namespace Celeste.Inventory
{
    [CreateAssetMenu(fileName = nameof(InventoryItem), menuName = "Celeste/Inventory/Inventory Item")]
    public class InventoryItem : ScriptableObject, IGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get { return guid; }
        }

        public string DisplayName
        {
            get { return displayName; }
        }

        public Sprite Sprite
        {
            get { return sprite; }
        }

        [SerializeField] private int guid;
        [SerializeField] private string displayName;
        [SerializeField] private Sprite sprite;

        #endregion
    }
}
