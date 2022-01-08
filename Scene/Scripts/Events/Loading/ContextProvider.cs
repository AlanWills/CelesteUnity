using Celeste.Scene.Events;
using System.Collections;
using UnityEngine;

namespace Celeste.Scene
{
    public abstract class ContextProvider : ScriptableObject
    {
        public abstract Context Create();
    }
}