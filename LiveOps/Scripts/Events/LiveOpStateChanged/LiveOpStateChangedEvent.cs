using System;
using Celeste.LiveOps;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public readonly struct LiveOpStateChangedArgs
    {
        public readonly LiveOp LiveOp;
        public readonly ValueChangedArgs<LiveOpState> State;

        public LiveOpStateChangedArgs(LiveOp liveOp, ValueChangedArgs<LiveOpState> state)
        {
            LiveOp = liveOp;
            State = state;
        }
    }
    
    [Serializable]
    public class LiveOpStateChangedUnityEvent : UnityEvent<LiveOpStateChangedArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(LiveOpStateChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Live Ops/Live Op State Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class LiveOpStateChangedEvent : ParameterisedEvent<LiveOpStateChangedArgs> { }
    
    [Serializable]
    public class GuaranteedLiveOpStateChangedEvent : GuaranteedParameterisedEvent<LiveOpStateChangedEvent, LiveOpStateChangedArgs> { }
}