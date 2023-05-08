using Celeste.Components;
using Celeste.Rewards.Catalogue;
using Celeste.Rewards.Objects;

namespace Celeste.LiveOps
{
    public interface ILiveOpCompletionReward
    {
        int GetCompletionRewardGuid(Instance instance);
        Reward GetCompletionReward(Instance instance, RewardCatalogue rewardCatalogue);

        bool HasCompletionRewardBeenAwarded(Instance instance);
        void AwardCompletionReward(Instance instance, RewardCatalogue rewardCatalogue, int multiplier);
    }
}
