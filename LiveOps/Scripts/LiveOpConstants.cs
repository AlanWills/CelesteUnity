﻿using Celeste.Components;
using Celeste.Rewards.Catalogue;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Celeste.LiveOps
{
    public static class LiveOpConstants
    {
        public static InterfaceHandle<ILiveOpTimer> NO_TIMER = new InterfaceHandle<ILiveOpTimer>()
        {
            iFace = new NoLiveOpTimer(),
            instance = new Instance(new ComponentData(), new ComponentEvents())
        };

        public static InterfaceHandle<ILiveOpProgress> NO_PROGRESS = new InterfaceHandle<ILiveOpProgress>()
        {
            iFace = new NoLiveOpProgress(),
            instance = new Instance(new ComponentData(), new ComponentEvents())
        };

        public static InterfaceHandle<ILiveOpAssets> NO_ASSETS = new InterfaceHandle<ILiveOpAssets>()
        {
            iFace = new NoLiveOpAssets(),
            instance = new Instance(new ComponentData(), new ComponentEvents())
        };

        public static readonly Reward[] EMPTY_REWARDS_LIST = new Reward[0];
    }
}