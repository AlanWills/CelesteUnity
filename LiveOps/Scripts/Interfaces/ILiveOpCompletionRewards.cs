using Celeste.Components;
using Celeste.Rewards.Catalogue;
using System.Collections.Generic;

namespace Celeste.LiveOps
{
    public interface ILiveOpCompletionRewards
    {
        IReadOnlyList<int> GetCompletionRewardGuids(Instance instance);
        IReadOnlyList<Reward> GetCompletionRewards(Instance instance, RewardCatalogue rewardCatalogue);

        void AwardCompletionRewards(Instance instance, RewardCatalogue rewardCatalogue);
    }
}
