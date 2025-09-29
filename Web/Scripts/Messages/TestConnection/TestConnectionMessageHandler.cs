using UnityEngine;

namespace Celeste.Web.Messages.TestConnection
{
    [CreateAssetMenu(fileName = nameof(TestConnectionMessageHandler), menuName = CelesteMenuItemConstants.WEB_MENU_ITEM + "Message Handlers/Test Connection", order = CelesteMenuItemConstants.WEB_MENU_ITEM_PRIORITY)]
    public class TestConnectionMessageHandler : NetworkingMessageHandler
    {
        public override bool CanHandle(NetworkingMessage message)
        {
            return message.Payload is TestConnectionPayload;
        }

        public override void Handle(NetworkingMessage message)
        {
            TestConnectionPayload testConnectionPayload = (TestConnectionPayload)message.Payload;
            UnityEngine.Debug.Log(testConnectionPayload.Message, CelesteLog.Web);
        }
    }
}