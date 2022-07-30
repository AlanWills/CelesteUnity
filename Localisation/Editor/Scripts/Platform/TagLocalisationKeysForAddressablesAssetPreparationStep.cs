using Celeste.Localisation;
using CelesteEditor.BuildSystem.Steps;
using CelesteEditor.Tools;
using UnityEngine;

namespace CelesteEditor.Localisation.Platform
{
    [CreateAssetMenu(fileName = nameof(TagLocalisationKeysForAddressablesAssetPreparationStep), menuName = "Celeste/Localisation/Asset Preparation/Tag Localisation Keys For Addressables")]
    public class TagLocalisationKeysForAddressablesAssetPreparationStep : AssetPreparationStep
    {
        [SerializeField] private string group;
        [SerializeField] private string label;

        public override void Execute()
        {
            foreach (LocalisationKey localisationKey in AssetUtility.FindAssets<LocalisationKey>())
            {
                localisationKey.SetAddressableInfo(group);
                localisationKey.SetAddressableLabel(group, label, true);
            }
        }
    }
}
