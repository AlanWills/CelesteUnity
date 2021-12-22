using Celeste.Events;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Loading.Events
{
    [Serializable]
    public class LoadJobUnityEvent : UnityEvent<LoadJob> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(LoadJobEvent), menuName = "Celeste/Events/Loading/Load Job Event")]
    public class LoadJobEvent : ParameterisedEvent<LoadJob>
    {
    }
}
