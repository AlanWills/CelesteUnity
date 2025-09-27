using System;
using Celeste.Objects;
using UnityEngine;

namespace Celeste.Web.Messages
{
    [CreateAssetMenu(fileName = nameof(NetworkingMessageSerializationFactory), menuName = CelesteMenuItemConstants.WEB_MENU_ITEM + "Message Serialization/Factory", order = CelesteMenuItemConstants.WEB_MENU_ITEM_PRIORITY)]
    public class NetworkingMessageSerializationFactory : ListScriptableObject<NetworkingMessageSerializationFactory.MessageIdToSerialization>, INetworkingMessageDeserializer, INetworkingMessageSerializer
    {
        #region Message Factory

        [Serializable]
        public struct MessageIdToSerialization
        {
            public bool IsValid => Id != 0 && Serializer != null && Deserializer != null;
            
            public int Id;
            public NetworkingMessageSerializer Serializer;
            public NetworkingMessageDeserializer Deserializer;
        }
        
        #endregion
        
        public string Serialize<T>(NetworkingMessage<T> message)
        {
            if (!message.IsValid)
            {
                UnityEngine.Debug.LogError($"Message {message.Id} is invalid.  It will fail to serialize and most likely be ignored.", CelesteLog.Web);
                return string.Empty;
            }
            
            var messageIdToSerialization = FindItem(x => x.Id == message.Id);
            if (messageIdToSerialization.IsValid)
            {
                return messageIdToSerialization.Serializer.Serialize(message);
            }

            UnityEngine.Debug.LogError($"Did not find serializer in factory for message id {message.Id}.  Serialization has failed...", CelesteLog.Web);
            return string.Empty;
        }
        
        public NetworkingMessage Deserialize(string message)
        {
            NetworkingMessageId id = JsonUtility.FromJson<NetworkingMessageId>(message);
            var messageIdToDeserializer = FindItem(x => x.Id == id.Id);

            if (messageIdToDeserializer.IsValid)
            {
                return messageIdToDeserializer.Deserializer.Deserialize(message);
            }

            UnityEngine.Debug.LogError($"Did not find deserializer in factory for message id {id.Id}.  Deserialization has failed...", CelesteLog.Web);
            return NetworkingMessage.Invalid;
        }
    }
}