using System;
using System.Collections.Generic;
using Celeste.Objects;
using UnityEngine;

namespace Celeste.Web.Messages
{
    [CreateAssetMenu(fileName = nameof(NetworkingMessageHandlerFactory), menuName = CelesteMenuItemConstants.WEB_MENU_ITEM + "Message Handlers/Factory", order = CelesteMenuItemConstants.WEB_MENU_ITEM_PRIORITY)]
    public class NetworkingMessageHandlerFactory : ListScriptableObject<NetworkingMessageHandlerFactory.MessageIdToHandler>, INetworkingMessageHandler
    {
        #region Message Factory

        [Serializable]
        public struct MessageIdToHandler
        {
            public bool IsValid => Id != 0 && Handler != null;
            
            public int Id;
            public NetworkingMessageHandler Handler;
        }
        
        #endregion
        
        [SerializeField] private NetworkingMessageHandler defaultHandler; 
        
        public bool CanHandle(NetworkingMessage message)
        {
            if (!message.IsValid)
            {
                return false;
            }
            
            var messageIdToHandler = FindItem(x => x.Id == message.Id);
            if (messageIdToHandler.IsValid)
            {
                return messageIdToHandler.Handler.CanHandle(message);
            }

            if (defaultHandler != null)
            {
                return defaultHandler.CanHandle(message);
            }

            return false;
        }

        public void Handle(NetworkingMessage message)
        {
            if (!message.IsValid)
            {
                UnityEngine.Debug.LogError($"Message {message.Id} is invalid.  It has failed to serialize, cannot be handled and so will most likely be ignored.", CelesteLog.Web);
                return;
            }
            
            var messageIdToHandler = FindItem(x => x.Id == message.Id);
            if (messageIdToHandler.IsValid)
            {
                messageIdToHandler.Handler.Handle(message);
                return;
            }

            if (defaultHandler != null)
            {
                UnityEngine.Debug.Log($"Did not find custom handler for message id {message.Id} in factory, so falling back to default handler {defaultHandler.name}.", CelesteLog.Web);
                defaultHandler.Handle(message);
                return;
            }
            
            UnityEngine.Debug.LogError($"Did not find matching handler or default handler in factory, so cannot handle message {message.Id}.", CelesteLog.Web);
        }
    }
}