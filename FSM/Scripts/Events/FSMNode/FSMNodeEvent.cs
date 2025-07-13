using System;
using UnityEngine;
using UnityEngine.Events;
using Celeste.Events;

namespace Celeste.FSM
{
    [Serializable]
    public class FSMNodeUnityEvent : UnityEvent<FSMNode> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(FSMNodeEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "FSM/FSM Node Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class FSMNodeEvent : ParameterisedEvent<FSMNode> { }
    
    [Serializable]
    public class GuaranteedFSMNodeEvent : GuaranteedParameterisedEvent<FSMNodeEvent, FSMNode> { }
}