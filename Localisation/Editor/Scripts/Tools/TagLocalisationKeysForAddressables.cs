using Celeste;
using Celeste.DataImporters.ImportSteps;
using Celeste.Localisation.Catalogue;
using CelesteEditor.Tools;
using UnityEngine;

namespace CelesteEditor.Localisation.Tools
{
    [CreateAssetMenu(
        fileName = nameof(TagLocalisationKeysForAddressables), 
        menuName = CelesteMenuItemConstants.DATAIMPORTERS_MENU_ITEM + "Tag Localisation Keys For Addressables",
        order = CelesteMenuItemConstants.DATAIMPORTERS_MENU_ITEM_PRIORITY)]
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