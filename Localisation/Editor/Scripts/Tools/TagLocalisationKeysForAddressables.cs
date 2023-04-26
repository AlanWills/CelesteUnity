using Celeste.DataImporters.ImportSteps;
using Celeste.Localisation.Catalogue;
using CelesteEditor.Tools;
using UnityEngine;

namespace CelesteEditor.Localisation.Tools
{
    [CreateAssetMenu(fileName = nameof(TagLocalisationKeysForAddressables), menuName = "Celeste/Data Importers/Tag Localisation Keys For Addressables")]
    public class TagLocalisationKeysForAddressables : ImportStep
    {
        #region Properties and Fields

        [SerializeField] private string group;
        [SerializeField] private string label;
        [SerializeField] private LocalisationKeyCatalogue localisationKeyCatalogue;

        #endregion

        public override void Execute()
        {
            foreach (var keyPair in localisationKeyCatalogue.Items)
            {
                Debug.Assert(keyPair.Value != null, $"Found a null localisation key in the catalogue {localisationKeyCatalogue.name}");
                if (keyPair.Value != null)
                {
                    keyPair.Value.SetAddressableInfo(group);
                    keyPair.Value.SetAddressableLabel(group, label, true);
                }
            }
        }
    }
}