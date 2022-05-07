using Celeste.DataImporters.ImportSteps;
using Celeste.Localisation;
using Celeste.Localisation.Catalogue;
using UnityEngine;

namespace CelesteEditor.Localisation.Tools
{
    [CreateAssetMenu(fileName = nameof(ClearLanguageLocalisation), menuName = "Celeste/Data Importers/Clear Language Localisation")]
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