using Celeste;
using CelesteEditor.Tools;
using System.IO;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    [CreateAssetMenu(
        fileName = nameof(DeleteAddressablesRemoteDirectory), 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "Asset Preparation/Delete Addressables Remote Directory",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class DeleteAddressablesRemoteDirectory : AssetPreparationStep
    {
        public override void Execute()
        {
            string buildDir = AddressablesExtensions.GetAddressablesRemoteBuildPath();

            if (Directory.Exists(buildDir))
            {
                Directory.Delete(buildDir, true);
            }
        }
    }
}
