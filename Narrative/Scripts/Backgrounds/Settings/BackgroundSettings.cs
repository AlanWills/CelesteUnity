using Celeste.Assets;
using System;
using System.Collections;
using Celeste.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Narrative.Backgrounds.Settings
{
    public abstract class BackgroundSettings : ScriptableObject, IHasAssets
    {
        public abstract bool ShouldLoadAssets();
        public abstract IEnumerator LoadAssets();

        public abstract Background FindBackgroundByGuid(int guid);

        public abstract void AddSetBackgroundListener(UnityAction<SetBackgroundEventArgs> background);
        public abstract void RemoveSetBackgroundListener(UnityAction<SetBackgroundEventArgs> background);
    }
}
