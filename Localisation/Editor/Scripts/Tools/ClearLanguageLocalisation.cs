using Celeste;
using Celeste.DataImporters.ImportSteps;
using Celeste.Localisation;
using Celeste.Localisation.Catalogue;
using UnityEngine;

namespace CelesteEditor.Localisation.Tools
{
    [CreateAssetMenu(
        fileName = nameof(ClearLanguageLocalisation), 
        menuName = CelesteMenuItemConstants.DATAIMPORTERS_MENU_ITEM + "Clear Language Localisation",
        order = CelesteMenuItemConstants.DATAIMPORTERS_MENU_ITEM_PRIORITY)]
    public class ClearLanguageLocalisation : ImportStep
    {
        #region Properties and Fields

        [SerializeField] private LanguageCatalogue languageCatalogue;

        #endregion

        public override void Execute()
        {
            foreach (Language language in languageCatalogue.Items)
            {
                language.ClearEntries();
            }
        }
    }
}