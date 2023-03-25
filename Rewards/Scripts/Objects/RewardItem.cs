using Celeste.Objects;
using Celeste.Parameters;
using Celeste.Wallet;
using UnityEngine;

namespace Celeste.Rewards.Objects
{
    public abstract class RewardItem : ScriptableObject, IGuid, IInitializable
    {
        #region Properties and Fields

        public int Guid
        {
            get => guid;
            set => guid = value;
        }

        public int Quantity => quantity.Value;
        
        public abstract Sprite Icon { get; }

        [SerializeField] protected int guid;
        [SerializeField] protected IntReference quantity;

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

        public abstract void AwardReward();
    }
}
