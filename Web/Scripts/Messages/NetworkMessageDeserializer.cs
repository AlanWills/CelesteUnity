using UnityEngine;

namespace Celeste.Web.Messages
{
    public abstract class NetworkingMessageDeserializer : ScriptableObject, INetworkMessageDeserializer
    {
        public abstract NetworkingMessage<T> Deserialize<T>(string message);
    }
}