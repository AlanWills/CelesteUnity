using Celeste.Objects;
using Celeste.Rewards.Objects;
using UnityEngine;

namespace Celeste.Rewards.Catalogue
{
    [CreateAssetMenu(fileName = nameof(RewardItemCatalogue), order = CelesteMenuItemConstants.REWARDS_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.REWARDS_MENU_ITEM + "Reward Item Catalogue")]
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
