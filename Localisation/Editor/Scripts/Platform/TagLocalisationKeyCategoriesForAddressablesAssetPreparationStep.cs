using Celeste;
using Celeste.Localisation;
using Celeste.Tools;
using CelesteEditor.BuildSystem.Steps;
using CelesteEditor.Tools;
using UnityEngine;

namespace CelesteEditor.Localisation.Platform
{
    [CreateAssetMenu(fileName = nameof(TagLocalisationKeyCategoriesForAddressablesAssetPreparationStep), menuName = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM + "Asset Preparation/Tag Localisation Key Categories For Addressables", order = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM_PRIORITY)]
    public class TagLocalisationKeyCategoriesForAddressablesAssetPreparationStep : AssetPreparationStep
    {
        [SerializeField] private string group;
        [SerializeField] private string label;

        public override void Execute()
        {
            foreach (LocalisationKeyCategory localisationKeyCategory in EditorOnly.FindAssets<LocalisationKeyCategory>())
            {
                localisationKeyCategory.SetAddressableInfo(group);
                localisationKeyCategory.SetAddressableLabel(group, label, true);
            }
        }
    }
}
