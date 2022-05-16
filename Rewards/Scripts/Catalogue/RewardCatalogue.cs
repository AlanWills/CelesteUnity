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
    }
}
