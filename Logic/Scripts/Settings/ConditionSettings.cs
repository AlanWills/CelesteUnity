using Celeste.Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Logic.Settings
{
    public abstract class ConditionSettings : ScriptableObject, IHasAssets
    {
        #region Properties and Fields

        public abstract IEnumerable<Condition> Conditions { get; }

        #endregion

        public abstract bool ShouldLoadAssets();
        public abstract IEnumerator LoadAssets();
    }
}
