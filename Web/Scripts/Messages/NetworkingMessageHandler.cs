using UnityEngine;

namespace Celeste.Web.Messages
{
    public abstract class NetworkingMessageHandler : ScriptableObject, INetworkingMessageHandler
    {
        public abstract bool CanHandle(NetworkingMessage message);
        public abstract void Handle(NetworkingMessage message);
    }
}