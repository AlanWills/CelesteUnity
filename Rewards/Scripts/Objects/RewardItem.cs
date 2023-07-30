using Celeste.Objects;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Rewards.Objects
{
    public abstract class RewardItem : ScriptableObject, IIntGuid, IInitializable
    {
        #region Properties and Fields

        public int Guid
        {
            get => guid;
            set => guid = value;
        }

        public int Quantity => quantity.Value;
        public bool CanBeMultiplied => canBeMultiplied;
        
        public abstract Sprite Icon { get; }

        [SerializeField] protected int guid;
        [SerializeField] protected IntReference quantity;
        [SerializeField] private bool canBeMultiplied;

        #endregion

        public void Initialize()
        {
            if (quantity == null)
            {
                quantity = CreateInstance<IntReference>();
                quantity.name = "RewardQuantity";
                quantity.hideFlags = HideFlags.HideInHierarchy;
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.AddObjectToAsset(quantity, this);
#endif
            }
            
            DoInitialize();
        }

        protected virtual void DoInitialize()
        {
        }

        public abstract void AwardReward(int multiplier);
    }
}
