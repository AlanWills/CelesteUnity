using Celeste.Objects;
using Celeste.Rewards.Objects;
using UnityEngine;

namespace Celeste.Rewards.Catalogue
{
    [CreateAssetMenu(fileName = nameof(RewardCatalogue), order = CelesteMenuItemConstants.REWARDS_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.REWARDS_MENU_ITEM + "Reward Catalogue")]
    public class RewardCatalogue : ListScriptableObject<Reward>
    {
        public Reward FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }

        public Reward MustFindByGuid(int guid)
        {
            Reward reward = FindByGuid(guid);
            UnityEngine.Debug.Assert(reward != null, $"Could not find {nameof(Reward)} with guid {guid} in catalogue.");

            return reward;
        }
    }
}
