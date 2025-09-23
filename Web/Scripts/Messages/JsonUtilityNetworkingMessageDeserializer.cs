using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Web.Messages
{
    [CreateAssetMenu(fileName = nameof(JsonUtilityNetworkingMessageDeserializer), menuName = CelesteMenuItemConstants.WEB_MENU_ITEM + "Deserializers/Json Utility", order = CelesteMenuItemConstants.WEB_MENU_ITEM_PRIORITY)]
    public class JsonUtilityNetworkingMessageDeserializer : NetworkingMessageDeserializer
    {
        #region Message Factory

        [Serializable]
        private struct MessageIdToDeserializer
        {
            public bool IsValid => Id != 0 && Deserializer != null;
            
            public int Id;
            public NetworkingMessageDeserializer Deserializer;
        }
        
        #endregion
        
        [SerializeField] private List<MessageIdToDeserializer> messageDeserializers = new();
        
        public override NetworkingMessage<T> Deserialize<T>(string message)
        {
            NetworkingMessageId id = JsonUtility.FromJson<NetworkingMessageId>(message);
            var messageIdToDeserializer = messageDeserializers.Find(x => x.Id == id.Id);
            return messageIdToDeserializer.IsValid ? messageIdToDeserializer.Deserializer.Deserialize<T>(message) : NetworkingMessage<T>.Invalid;
        }
    }
}