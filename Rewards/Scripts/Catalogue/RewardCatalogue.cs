using Celeste.Objects;
using UnityEngine;

namespace Celeste.Rewards.Catalogue
{
    [CreateAssetMenu(fileName = nameof(RewardCatalogue), menuName = "Celeste/Rewards/Reward Catalogue")]
    public class RewardCatalogue : ListScriptableObject<Reward>
    {
        public Reward FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }

        public Reward MustFindByGuid(int guid)
        {
            Reward reward = FindByGuid(guid);
            UnityEngine.Debug.Assert(reward != null, $"Could not find reward with guid {guid} in catalogue.");

            return reward;
        }
    }
}
