using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public struct TooltipArgs
    {
        public string text;
        public Vector3 position;
        public bool isWorldSpace;
    }

    [Serializable]
    public class ShowTooltipUnityEvent : UnityEvent<TooltipArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(ShowTooltipEvent), menuName = "Celeste/Events/UI/Show Tooltip Event")]
    public class ShowTooltipEvent : ParameterisedEvent<TooltipArgs> 
    {
    }
}
