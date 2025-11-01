using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Celeste.Events;

namespace Celeste.Loading
{
    [Serializable]
    public class TaskUnityEvent : UnityEvent<Task> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(TaskEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Loading/Task Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class TaskEvent : ParameterisedEvent<Task> { }
    
    [Serializable]
    public class GuaranteedTaskEvent : GuaranteedParameterisedEvent<TaskEvent, Task> { }
}