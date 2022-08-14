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

    [CreateAssetMenu(fileName = nameof(HudLogMessageList), menuName = "Celeste/Log/Hud Log Message List")]
    public class HudLogMessageList : ListScriptableObject<HudLogMessage>
    {
    }
}
