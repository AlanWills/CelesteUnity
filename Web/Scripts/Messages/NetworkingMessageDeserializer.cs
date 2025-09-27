using UnityEngine;

namespace Celeste.Web.Messages
{
    public abstract class NetworkingMessageDeserializer : ScriptableObject, INetworkingMessageDeserializer
    {
        public abstract NetworkingMessage Deserialize(string message);
    }
}