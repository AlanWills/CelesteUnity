using System;
using Celeste.DataStructures;
using Celeste.Debug.Events;
using Celeste.Objects;
using Celeste.Tools.Attributes.GUI;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

namespace Celeste.Debug.Tools
{
    [CreateAssetMenu(
        fileName = nameof(ConnectionMessageList), 
        menuName = CelesteMenuItemConstants.DEBUG_MENU_ITEM + "Connection Message List",  
        order = CelesteMenuItemConstants.DEBUG_MENU_ITEM_PRIORITY)]
    public class ConnectionMessageList : ListScriptableObject<ConnectionMessageList.GuidToEventStruct>
    {
        #region Guid To Event Struct

        [Serializable]
        public struct GuidToEventStruct
        {
            public bool IsValid => MessageGuid != Guid.Empty && DataReceived != null;
            public string MessageType => messageType;
            public Guid MessageGuid => new(messageGuid);
            public EditorConnectionDataEvent DataReceived => dataReceived;
            
            [SerializeField] private string messageType;
            [SerializeField, StringAsSystemGuid] private string messageGuid;
            [SerializeField] private EditorConnectionDataEvent dataReceived;
        }
        
        #endregion
    }
}