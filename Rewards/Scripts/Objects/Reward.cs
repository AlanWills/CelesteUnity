using Celeste.Logic;
using Celeste.Objects;
using Celeste.Tools.Attributes.GUI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Rewards.Objects
{
    [Serializable]
    public struct RewardItemInfo
    {
        public bool ShouldAward => !isConditional || shouldRewardCondition.IsMet;

        public bool isConditional;
        [ShowIf(nameof(isConditional))] public Condition shouldRewardCondition;
        public RewardItem rewardItem;

        public void AwardReward(int multiplier)
        {
            if (ShouldAward)
            {
                rewardItem.AwardReward(multiplier);
            }
        }
    }

    [CreateAssetMenu(fileName = nameof(Reward), menuName = "Celeste/Rewards/Reward")]
    public class Reward : ListScriptableObject<RewardItemInfo>, IIntGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get => guid;
            set => guid = value;
        }

        public IReadOnlyList<RewardItem> AwardableRewards
        {
            get
            {
                List<RewardItem> rewardItems = new List<RewardItem>();

                foreach (var itemInfo in this)
                {
                    if (itemInfo.ShouldAward)
                    {
                        rewardItems.Add(itemInfo.rewardItem);
                    }
                }

                return rewardItems;
            }
        }

        [SerializeField] private int guid;

        #endregion
        
        public void AwardReward(int multiplier)
        {
            foreach (RewardItemInfo rewardItem in Items)
            {
                rewardItem.AwardReward(multiplier);
            }
        }
    }
}
