using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class GameObjectUnityEvent : UnityEvent<GameObject> { }

    [Serializable]
    [CreateAssetMenu(fileName = "GameObjectEvent", menuName = "Celeste/Events/GameObject Event")]
    public class GameObjectEvent : ParameterisedEvent<GameObject>
    {
    }
}
