using Celeste.Logic.Catalogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Logic.Settings
{
    [CreateAssetMenu(fileName = nameof(ConditionSettingsUsingBakedAssets), menuName = CelesteMenuItemConstants.LOGIC_MENU_ITEM + "Settings/Baked Assets", order = CelesteMenuItemConstants.LOGIC_MENU_ITEM_PRIORITY)]
    public class ConditionSettingsUsingBakedAssets : ConditionSettings
    {
        #region Properties and Fields

        public override IEnumerable<Condition> Conditions => conditionCatalogue;

        [SerializeField] private ConditionCatalogue conditionCatalogue;

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
