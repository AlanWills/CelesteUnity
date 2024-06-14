using Celeste.Objects;
using UnityEngine;

namespace Celeste.Log.DataStructures
{
    [CreateAssetMenu(fileName = nameof(LogMessageList), menuName = CelesteMenuItemConstants.LOG_MENU_ITEM + "Log Message List", order = CelesteMenuItemConstants.LOG_MENU_ITEM_PRIORITY)]
    public class LogMessageList : ListScriptableObject<LogMessage>
    {
    }
}
