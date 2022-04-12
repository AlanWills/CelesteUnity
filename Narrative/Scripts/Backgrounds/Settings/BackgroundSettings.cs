using Celeste.Assets;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Narrative.Backgrounds.Settings
{
    public abstract class BackgroundSettings : ScriptableObject, IHasAssets
    {
        #region Properties and Fields

        public abstract int CurrentBackgroundGuid { get; set; }

        #endregion

        public abstract bool ShouldLoadAssets();
        public abstract IEnumerator LoadAssets();

        public abstract Background FindBackgroundByGuid(int guid);

        public Background FindCurrentBackground()
        {
            return FindBackgroundByGuid(CurrentBackgroundGuid);
        }

        public abstract void AddSetBackgroundListener(Action<Background> background);
        public abstract void RemoveSetBackgroundListener(Action<Background> background);
    }
}
