using Celeste.Components;
using UnityEngine;

namespace Celeste.LiveOps.Components
{
    [CreateAssetMenu(fileName = nameof(AchievementProgress), menuName = "Celeste/Live Ops/Progress/Achievement")]
    public class AchievementProgress : Celeste.Components.Component, ILiveOpProgress
    {
        public bool HasProgress(Instance instance)
        {
            // TODO
            return true;
        }
    }
}
