using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public struct GameObjectClickEventArgs
    {
        public GameObject gameObject;
        public Vector3 clickWorldPosition;

        public override string ToString()
        {
            return $"{(gameObject != null ? gameObject.name : "<null>")}, {clickWorldPosition}";
        }
    }

    [Serializable]
    public class GameObjectClickUnityEvent : UnityEvent<GameObjectClickEventArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = "GameObjectClickEvent", menuName = "Celeste/Events/Input/GameObject Click Event")]
    public class GameObjectClickEvent : ParameterisedEvent<GameObjectClickEventArgs>
    {
    }
}
