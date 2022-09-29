using Celeste.Parameters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Options.Settings
{
    [CreateAssetMenu(fileName = nameof(OptionsSettingsUsingAssets), menuName = "Celeste/Options/Settings/Using Assets")]
    public class OptionsSettingsUsingAssets : OptionsSettings
    {
        #region Properties and Fields

        public override IEnumerable<BoolOption> BoolOptions => boolOptions;
        public override IEnumerable<FloatOption> FloatOptions => floatOptions;
        public override IEnumerable<IntOption> IntOptions => intOptions;
        public override IEnumerable<StringOption> StringOptions => stringOptions;

        [SerializeField] private BoolOptionList boolOptions;
        [SerializeField] private FloatOptionList floatOptions;
        [SerializeField] private IntOptionList intOptions;
        [SerializeField] private StringOptionList stringOptions;

        #endregion

        public override bool ShouldLoadAssets()
        {
            return false;
        }

        public override IEnumerator LoadAssets()
        {
            yield break;
        }
    }
}
