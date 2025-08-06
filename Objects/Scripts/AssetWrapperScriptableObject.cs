using UnityEngine;

namespace Celeste.Objects
{
    public abstract class AssetWrapperScriptableObject<T> : ScriptableObject where T : Object
    {
        public T asset;
    }
}