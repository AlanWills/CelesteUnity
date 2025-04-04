using System;
using System.Collections.Generic;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine;

namespace Celeste.Debug.Tools
{
    [CreateAssetMenu(
        fileName = nameof(PlayerConnectionRecord), 
        menuName = CelesteMenuItemConstants.DEBUG_MENU_ITEM + "Player Connection Record",  
        order = CelesteMenuItemConstants.DEBUG_MENU_ITEM_PRIORITY)]
    public class PlayerConnectionRecord : ScriptableObject
    {
        #region Properties and Fields

        public bool IsConnected => PlayerConnection.instance.isConnected;
        public bool IsSetup { get; private set; }

        [SerializeField] private ConnectionMessageList availableMessages;

        #endregion
        
        public void SetupConnectionToEditor()
        {
            if (IsSetup)
            {
                UnityEngine.Debug.LogError("Player already setup for connection to Editor.", CelesteLog.Debug);
                return;
            }
            
            IsSetup = true;
            UnityEngine.Debug.Log("Player connection to Editor setup.", CelesteLog.Debug);

            foreach (var messageAndEvent in availableMessages)
            {
                if (!messageAndEvent.IsValid)
                {
                    UnityEngine.Debug.LogAssertion("Detected invalid editor connection message.  Skipping...", CelesteLog.Debug);
                    continue;
                }
                
                UnityEngine.Debug.Log($"Registering for editor connection message {messageAndEvent.MessageType} {messageAndEvent.MessageGuid}", CelesteLog.Debug);
                PlayerConnection.instance.Register(messageAndEvent.MessageGuid, messageAndEvent.DataReceived.Invoke);
            }
        }

        public void TearDownConnectionToEditor()
        {
            IsSetup = false;
            UnityEngine.Debug.Log("Player connection to Editor torn down.", CelesteLog.Debug);
            
            foreach (var messageAndEvent in availableMessages)
            {
                if (!messageAndEvent.IsValid)
                {
                    continue;
                }

                UnityEngine.Debug.Log($"Unregistering from editor connection message {messageAndEvent.MessageType} {messageAndEvent.MessageGuid}", CelesteLog.Debug);
                PlayerConnection.instance.Unregister(messageAndEvent.MessageGuid, messageAndEvent.DataReceived.Invoke);
            }
        }
    }
}