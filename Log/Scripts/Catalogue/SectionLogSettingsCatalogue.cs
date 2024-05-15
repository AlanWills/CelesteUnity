using Celeste.Objects;
using UnityEngine;

namespace Celeste.Log
{
    [CreateAssetMenu(fileName = nameof(SectionLogSettingsCatalogue), menuName = CelesteMenuItemConstants.LOG_MENU_ITEM + "Section Log Settings Catalogue", order = CelesteMenuItemConstants.LOG_MENU_ITEM_PRIORITY)]
    public class SectionLogSettingsCatalogue : ListScriptableObject<SectionLogSettings>, IAutomaticImportSettings
    {
        #region Properties and Fields

        public AutomaticImportBehaviour ImportBehaviour => importBehaviour;

        [SerializeField] private AutomaticImportBehaviour importBehaviour = AutomaticImportBehaviour.ImportAllAssets;

        #endregion

        public SectionLogSettings FindBySectionName(string sectionName)
        {
            return FindItem(x => string.CompareOrdinal(sectionName, x.SectionName) == 0);
        }

        public SectionLogSettings MustFindBySectionName(string sectionName)
        {
            SectionLogSettings sectionLogSettings = FindBySectionName(sectionName);
            UnityEngine.Debug.Assert(sectionLogSettings != null, $"Unable to find {nameof(SectionLogSettings)} with section name '{sectionName}'.");
            return sectionLogSettings;
        }
    }
}