using Celeste.Objects;
using Celeste.Rewards.Objects;
using UnityEngine;

namespace Celeste.Rewards.Catalogue
{
    [CreateAssetMenu(fileName = nameof(RewardItemCatalogue), menuName = "Celeste/Rewards/Reward Item Catalogue")]
    public class RewardItemCatalogue : ListScriptableObject<RewardItem>
    {
        public RewardItem FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }

        public RewardItem MustFindByGuid(int guid)
        {
            RewardItem rewardItem = FindByGuid(guid);
            UnityEngine.Debug.Assert(rewardItem != null, $"Could not find {nameof(RewardItem)} with guid {guid} in catalogue.");

            return rewardItem;
        }
    }
}
