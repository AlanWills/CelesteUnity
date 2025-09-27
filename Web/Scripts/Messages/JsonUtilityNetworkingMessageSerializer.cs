using UnityEngine;

namespace Celeste.Web.Messages
{
    [CreateAssetMenu(fileName = nameof(JsonUtilityNetworkingMessageSerializer), menuName = CelesteMenuItemConstants.WEB_MENU_ITEM + "Message Serializers/Json Utility", order = CelesteMenuItemConstants.WEB_MENU_ITEM_PRIORITY)]
    public class JsonUtilityNetworkingMessageSerializer : NetworkingMessageSerializer
    {
        public override string Serialize<T>(NetworkingMessage<T> networkingMessage)
        {
            return JsonUtility.ToJson(networkingMessage);
        }
    }
}