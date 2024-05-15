using Celeste.Objects;
using System;
using UnityEngine;

namespace Celeste.Log.DataStructures
{
    [Serializable]
    public struct HudLogMessage
    {
        public string message;
        public string callstack;
    }

    [CreateAssetMenu(fileName = nameof(HudLogMessageList), menuName = CelesteMenuItemConstants.LOG_MENU_ITEM + "Hud Log Message List", order = CelesteMenuItemConstants.LOG_MENU_ITEM_PRIORITY)]
    public class HudLogMessageList : ListScriptableObject<HudLogMessage>
    {
    }
}
