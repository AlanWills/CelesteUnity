using Celeste.Localisation;
using CelesteEditor.Platform.Steps;
using CelesteEditor.Tools;
using UnityEngine;

namespace CelesteEditor.Localisation.Platform
{
    [CreateAssetMenu(fileName = nameof(TagLocalisationKeyCategoriesForAddressablesAssetPreparationStep), menuName = "Celeste/Localisation/Asset Preparation/Tag Localisation Key Categories For Addressables")]
    public class TagLocalisationKeyCategoriesForAddressablesAssetPreparationStep : AssetPreparationStep
    {
        [SerializeField] private string group;
        [SerializeField] private string label;

        public override void Execute()
        {
            foreach (LocalisationKeyCategory localisationKeyCategory in AssetUtility.FindAssets<LocalisationKeyCategory>())
            {
                localisationKeyCategory.SetAddressableInfo(group);
                localisationKeyCategory.SetAddressableLabel(group, label, true);
            }
        }
    }
}
