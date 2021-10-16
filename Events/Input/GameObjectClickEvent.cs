using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    public struct GameObjectClickEventArgs
    {
        public GameObject gameObject;
        public Vector3 clickWorldPosition;

        public override string ToString()
        {
            return string.Format("{0}, {1}", gameObject != null ? gameObject.name : "<null>", clickWorldPosition);
        }
    }

    [Serializable]
    public class GameObjectClickUnityEvent : UnityEvent<GameObjectClickEventArgs> { }

    [Serializable]
    [CreateAssetMenu(fileName = "GameObjectClickEvent", menuName = "Celeste/Events/GameObject Click Event")]
    public class GameObjectClickEvent : ParameterisedEvent<GameObjectClickEventArgs>
    {
    }
}
