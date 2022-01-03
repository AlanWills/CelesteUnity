using CelesteEditor.Platform.Steps;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace CelesteEditor.Platform
{
    [CreateAssetMenu(fileName = nameof(SetAddressablesActiveProfileId), menuName = "Celeste/Platform/Asset Preparation/Set Addressables Active Profile Id")]
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