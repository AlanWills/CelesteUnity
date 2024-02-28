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
        public bool anchorToMouse;

        public static TooltipArgs AtFixedWorldPosition(Vector3 position, string text)
        {
            return new TooltipArgs
            {
                position = position,
                isWorldSpace = true,
                text = text,
                anchorToMouse = false
            };
        }

        public static TooltipArgs AtFixedScreenPosition(Vector3 position, string text)
        {
            return new TooltipArgs
            {
                position = position,
                isWorldSpace = false,
                text = text,
                anchorToMouse = false
            };
        }

        public static TooltipArgs AnchoredToMouse(string text)
        {
            return new TooltipArgs
            {
                position = Vector3.zero,
                isWorldSpace = false,
                text = text,
                anchorToMouse = true
            };
        }

        public override string ToString()
        {
            return $"{position} - {isWorldSpace} - {text}";
        }
    }

    [Serializable]
    public class ShowTooltipUnityEvent : UnityEvent<TooltipArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(ShowTooltipEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "UI/Show Tooltip Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class ShowTooltipEvent : ParameterisedEvent<TooltipArgs> 
    {
    }
}
