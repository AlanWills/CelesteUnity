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
    [CreateAssetMenu(fileName = "GameObjectEvent", menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "GameObject Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
    public class GameObjectEvent : ParameterisedEvent<GameObject>
    {
    }
}
