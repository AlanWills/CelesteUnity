using Celeste.Web.Messages;

namespace Celeste.Web.Objects
{
    public class DisabledNetworkingClient : INetworkingClient
    {
        public bool Exists => false;
        public bool HasNetworkObject => false;
        public ulong Id => ulong.MaxValue;
        
        public void SendMessageToServer<T>(NetworkingMessage<T> message)
        {
            UnityEngine.Debug.Log($"Client Networking disabled.  Discarding message: {message}.", CelesteLog.Web);
        }

        void INetworkingMessageReceiver.OnNetworkingMessageReceived(string rawMessage)
        {
            UnityEngine.Debug.Log($"Server Networking disabled.  Discarding message: {rawMessage}.", CelesteLog.Web);
        }
    }
}