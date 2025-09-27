using UnityEngine;

namespace Celeste.Web.Messages.TestConnection
{
    [CreateAssetMenu(fileName = nameof(TestConnectionMessageDeserializer), menuName = CelesteMenuItemConstants.WEB_MENU_ITEM + "Message Deserializers/Test Connection", order = CelesteMenuItemConstants.WEB_MENU_ITEM_PRIORITY)]
    public class TestConnectionMessageDeserializer : NetworkingMessageDeserializer
    {
        public override NetworkingMessage Deserialize(string message)
        {
            return JsonUtility.FromJson<NetworkingMessage<TestConnectionPayload>>(message).Typeless();
        }
    }
}