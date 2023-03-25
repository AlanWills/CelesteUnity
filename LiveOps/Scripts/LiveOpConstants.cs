using Celeste.Components;
using Celeste.Rewards.Catalogue;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Celeste.Rewards.Objects;

namespace Celeste.LiveOps
{
    public static class LiveOpConstants
    {
        public static InterfaceHandle<ILiveOpTimer> NO_TIMER = new()
        {
            iFace = new NoLiveOpTimer(),
            instance = new Instance(new ComponentData(), new ComponentEvents())
        };

        public static InterfaceHandle<ILiveOpProgress> NO_PROGRESS = new()
        {
            iFace = new NoLiveOpProgress(),
            instance = new Instance(new ComponentData(), new ComponentEvents())
        };

        public static InterfaceHandle<ILiveOpAssets> NO_ASSETS = new()
        {
            iFace = new NoLiveOpAssets(),
            instance = new Instance(new ComponentData(), new ComponentEvents())
        };

        public static readonly Reward NO_REWARD = null;
    }
}
