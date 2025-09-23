using UnityEngine;

namespace Celeste.Web.Messages
{
    public abstract class NetworkingMessageSerializer : ScriptableObject, INetworkingMessageSerializer
    {
        public abstract string Serialize<T>(NetworkingMessage<T> networkingMessage);
    }
}