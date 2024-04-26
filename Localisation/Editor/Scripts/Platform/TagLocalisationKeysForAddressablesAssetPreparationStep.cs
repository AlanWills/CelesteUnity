using Celeste;
using Celeste.Localisation;
using Celeste.Tools;
using CelesteEditor.BuildSystem.Steps;
using CelesteEditor.Tools;
using UnityEngine;

namespace CelesteEditor.Localisation.Platform
{
    [CreateAssetMenu(fileName = nameof(TagLocalisationKeysForAddressablesAssetPreparationStep), menuName = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM + "Asset Preparation/Tag Localisation Keys For Addressables", order = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM_PRIORITY)]
    public class TagLocalisationKeysForAddressablesAssetPreparationStep : AssetPreparationStep
    {
        [SerializeField] private string group;
        [SerializeField] private string label;

        public override void Execute()
        {
            foreach (LocalisationKey localisationKey in EditorOnly.FindAssets<LocalisationKey>())
            {
                localisationKey.SetAddressableInfo(group);
                localisationKey.SetAddressableLabel(group, label, true);
            }
        }
    }
}
