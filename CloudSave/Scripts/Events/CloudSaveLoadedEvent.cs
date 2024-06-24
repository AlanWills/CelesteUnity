using System;
using UnityEngine;
using UnityEngine.Events;
using Celeste.Events;
using Celeste.Persistence.Snapshots;

namespace Celeste.CloudSave
{
    [Serializable]
    public struct CloudSaveLoadedArgs
    {
        public DataSnapshot loadedData;
    }

    [Serializable]
    public class CloudSaveLoadedUnityEvent : UnityEvent<CloudSaveLoadedArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(CloudSaveLoadedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Cloud Save/Cloud Save Loaded Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class CloudSaveLoadedEvent : ParameterisedEvent<CloudSaveLoadedArgs> { }
    
    [Serializable]
    public class GuaranteedCloudSaveLoadedEvent : GuaranteedParameterisedEvent<CloudSaveLoadedEvent, CloudSaveLoadedArgs> { }
}