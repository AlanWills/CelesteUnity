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

        public abstract IEnumerable<BoolOption> BoolOptions { get; }
        public abstract IEnumerable<FloatOption> FloatOptions { get; }
        public abstract IEnumerable<IntOption> IntOptions { get; }
        public abstract IEnumerable<StringOption> StringOptions { get; }

        #endregion

        public abstract bool ShouldLoadAssets();
        public abstract IEnumerator LoadAssets();
    }
}
