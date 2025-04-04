using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
#if UNITY_EDITOR
using System.Text;
using UnityEditor.Networking.PlayerConnection;
#endif
using UnityEngine;

namespace Celeste.Debug.Tools
{
    [CreateAssetMenu(
        fileName = nameof(EditorConnectionRecord), 
        menuName = CelesteMenuItemConstants.DEBUG_MENU_ITEM + "Editor Connection Record",  
        order = CelesteMenuItemConstants.DEBUG_MENU_ITEM_PRIORITY)]
    public class EditorConnectionRecord : ScriptableObject
    {
        #region Properties and Fields

        public bool ConnectionSetup => connectionSetup;
        
        public int NumConnectedPlayers
        {
            get
            {
#if UNITY_EDITOR
                return EditorConnection.instance.ConnectedPlayers?.Count ?? 0;
#else
                return 0;
#endif
            }
        }
        
        [NotNull]
        public IEnumerable<int> ConnectedPlayers
        {
            get
            {
#if UNITY_EDITOR
                var connectedPlayers = EditorConnection.instance.ConnectedPlayers;
                if (connectedPlayers == null)
                {
                    yield break;
                }

                foreach (var player in connectedPlayers)
                {
                    yield return player.playerId;
                }
#else
                yield break;
#endif
            }
        }

        [SerializeField] private ConnectionMessageList availableMessages;

        [NonSerialized] private bool connectionSetup;

        #endregion
        
        [Conditional("UNITY_EDITOR")]
        public void SetupEditorConnection()
        {
            if (connectionSetup)
            {
                UnityEngine.Debug.LogWarning("Attempting to set up the Editor Connection, but it is already setup.  Ignoring...", CelesteLog.Debug);
                return;
            }

#if UNITY_EDITOR
            connectionSetup = true;
            
            EditorConnection.instance.Initialize();
            EditorConnection.instance.RegisterConnection(OnPlayerConnected);
            EditorConnection.instance.RegisterDisconnection(OnPlayerDisconnected);
            
            foreach (var messageAndEvent in availableMessages)
            {
                if (!messageAndEvent.IsValid)
                {
                    UnityEngine.Debug.LogAssertion("Detected invalid editor connection message.  Skipping...", CelesteLog.Debug);
                    continue;
                }
                EditorConnection.instance.Register(messageAndEvent.MessageGuid, messageAndEvent.DataReceived.Invoke);
            }
#endif
        }
        
        [Conditional("UNITY_EDITOR")]
        public void TeardownEditorConnection()
        {
#if UNITY_EDITOR
            connectionSetup = false;
            
            EditorConnection.instance.DisconnectAll();
            
            foreach (var messageAndEvent in availableMessages)
            {
                if (!messageAndEvent.IsValid)
                {
                    continue;
                }
                
                EditorConnection.instance.Unregister(messageAndEvent.MessageGuid, messageAndEvent.DataReceived.Invoke);
            }
            
            EditorConnection.instance.UnregisterDisconnection(OnPlayerDisconnected);
            EditorConnection.instance.UnregisterConnection(OnPlayerConnected);
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public void PingAll()
        {
            if (availableMessages.NumItems == 0)
            {
                UnityEngine.Debug.LogError("Unable to ping connected players - no available messages found.");
                return;
            }
            
            SendMessageToAll(availableMessages.GetItem(0).MessageGuid, "Ping From Editor!");
        }

        [Conditional("UNITY_EDITOR")]
        public void SendMessageToAll(string messageType, string message)
        {
#if UNITY_EDITOR
            // Don't send messages we don't know about
            var messageInfo = availableMessages.FindItem(x => string.CompareOrdinal(messageType, x.MessageType) == 0);
            if (!messageInfo.IsValid)
            {
                UnityEngine.Debug.LogError($"Attempting to send a message with type {messageType} and body {message}, but it is not registered as an available message.  Ignoring...", CelesteLog.Debug);
                return;
            }
            
            SendMessageToAll(messageInfo.MessageGuid, message);
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public void SendMessageToAll(Guid messageId, string message)
        {
#if UNITY_EDITOR
            // Don't send messages we don't know about
            if (!availableMessages.Exists(x => x.MessageGuid == messageId))
            {
                UnityEngine.Debug.LogError($"Attempting to send a message with id {messageId} and body {message}, but it is not registered as an available message.  Ignoring...", CelesteLog.Debug);
                return;
            }
            
            byte[] data = Encoding.UTF8.GetBytes(message);
            
            foreach (var player in ConnectedPlayers)
            {
                if (EditorConnection.instance.TrySend(messageId, data))
                {
                    UnityEngine.Debug.Log($"Message with id {messageId} successfully sent to Player {player}.", CelesteLog.Debug);
                }
                else
                {
                    UnityEngine.Debug.LogError($"Message with id {messageId} could not be sent to Player {player}.", CelesteLog.Debug);
                }
            }
#endif
        }
        
        #region Callbacks

        private void OnPlayerConnected(int playerId)
        {
            UnityEngine.Debug.Log($"Player Connected to Editor: {playerId}", CelesteLog.Debug);
        }

        private void OnPlayerDisconnected(int playerId)
        {
            UnityEngine.Debug.Log($"Player disconnected from Editor: {playerId}", CelesteLog.Debug);
        }

        #endregion
    }
}