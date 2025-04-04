using System;
using Celeste.DataStructures;
using UnityEngine;
using UnityEngine.Events;
using Celeste.Events;
using UnityEngine.Networking.PlayerConnection;

namespace Celeste.Debug.Events
{
    [Serializable]
    public class EditorConnectionDataUnityEvent : UnityEvent<IDataDictionary> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(EditorConnectionDataEvent),
        menuName = "Celeste/Debug/Events/Editor Connection Data Event")]
    public class EditorConnectionDataEvent : ParameterisedEvent<IDataDictionary>
    {
        public void Invoke(MessageEventArgs messageEventArgs)
        {
            byte[] data = messageEventArgs.data;
            if (data == null || data.Length == 0)
            {
                UnityEngine.Debug.LogError($"Ignoring message {name} as it had no data payload.", CelesteLog.Debug);
                return;
            }
  
            string dataAsString = System.Text.Encoding.UTF8.GetString(data);
            UnityEngine.Debug.Log($"Message {name} received with data {dataAsString}.", CelesteLog.Debug);
                
            IDataDictionary dataDictionary = new fsDataDictionary(dataAsString);
            Invoke(dataDictionary);
        }
    }
    
    [Serializable]
    public class GuaranteedEditorConnectionDataEvent : GuaranteedParameterisedEvent<EditorConnectionDataEvent, IDataDictionary> { }
}