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

        public override IEnumerable<BoolValue> BoolOptions => boolOptions;
        public override IEnumerable<FloatValue> FloatOptions => floatOptions;
        public override IEnumerable<IntValue> IntOptions => intOptions;
        public override IEnumerable<StringValue> StringOptions => stringOptions;

        [SerializeField] private BoolValueList boolOptions;
        [SerializeField] private FloatValueList floatOptions;
        [SerializeField] private IntValueList intOptions;
        [SerializeField] private StringValueList stringOptions;

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
