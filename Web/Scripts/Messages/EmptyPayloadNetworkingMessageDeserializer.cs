using UnityEngine;

namespace Celeste.Web.Messages
{
    [CreateAssetMenu(fileName = nameof(EmptyPayloadNetworkingMessageDeserializer), menuName = CelesteMenuItemConstants.WEB_MENU_ITEM + "Message Deserializers/Empty Payload", order = CelesteMenuItemConstants.WEB_MENU_ITEM_PRIORITY)]
    public class EmptyPayloadNetworkingMessageDeserializer : NetworkingMessageDeserializer
    {
        public override NetworkingMessage Deserialize(string message)
        {
            return new NetworkingMessage
            {
                Id = JsonUtility.FromJson<NetworkingMessageId>(message).Id
            };
        }
    }
}