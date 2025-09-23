using Celeste.Web.Messages;

namespace Celeste.Web.Objects
{
    public class DisabledNetworkingClient : INetworkingClient
    {
        public bool Exists => false;
        public bool HasNetworkObject => false;
        public ulong Id => 0;
        
        public void SendMessageToServer(string message)
        {
            UnityEngine.Debug.Log($"Client Networking disabled.  Discarding message: {message}.");
        }

        public void SendMessageToServer<T>(NetworkingMessage<T> message)
        {
            UnityEngine.Debug.Log($"Client Networking disabled.  Discarding message: {message}.");
        }
    }
}