using Celeste.Assets;
using Celeste.Parameters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Options.Settings
{
    public abstract class OptionsSettings : ScriptableObject, IHasAssets
    {
        #region Properties and Fields

        public abstract IEnumerable<BoolValue> BoolOptions { get; }
        public abstract IEnumerable<FloatValue> FloatOptions { get; }
        public abstract IEnumerable<IntValue> IntOptions { get; }
        public abstract IEnumerable<StringValue> StringOptions { get; }

        #endregion

        public abstract bool ShouldLoadAssets();
        public abstract IEnumerator LoadAssets();
    }
}
