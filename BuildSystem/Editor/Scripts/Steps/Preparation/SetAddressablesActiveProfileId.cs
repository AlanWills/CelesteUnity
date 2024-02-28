using Celeste;
using CelesteEditor.BuildSystem.Steps;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace CelesteEditor.BuildSystem
{
    [CreateAssetMenu(
        fileName = nameof(SetAddressablesActiveProfileId), 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "Asset Preparation/Set Addressables Active Profile Id",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class SetAddressablesActiveProfileId : AssetPreparationStep
    {
        public string profileId;

        public override void Execute()
        {
            Debug.Assert(AddressableAssetSettingsDefaultObject.SettingsExists, "AddressableAssetSettingsDefaultObject does not exist");
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            Debug.Assert(settings != null, "AddressableAssetSettingsDefaultObject is null");

            if (settings.profileSettings != null)
            {
                settings.activeProfileId = settings.profileSettings.GetProfileId(profileId);
            }

            Debug.Log($"Active Profile Id: {settings.activeProfileId}");
        }
    }
}