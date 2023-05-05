using Celeste.Logic;
using Celeste.Objects;
using Celeste.Tools.Attributes.GUI;
using System;
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

        public void AwardReward()
        {
            if (ShouldAward)
            {
                rewardItem.AwardReward();
            }
        }
    }

    [CreateAssetMenu(fileName = nameof(Reward), menuName = "Celeste/Rewards/Reward")]
    public class Reward : ListScriptableObject<RewardItemInfo>, IGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get => guid;
            set => guid = value;
        }

        [SerializeField] private int guid;

        #endregion
        
        public void AwardReward()
        {
            foreach (RewardItemInfo rewardItem in Items)
            {
                rewardItem.AwardReward();
            }
        }
    }
}
