using CelesteEditor.Tools;
using System.IO;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    [CreateAssetMenu(fileName = nameof(DeleteAddressablesRemoteDirectory), menuName = "Celeste/Build System/Asset Preparation/Delete Addressables Remote Directory")]
    public class DeleteAddressablesRemoteDirectory : AssetPreparationStep
    {
        public override void Execute()
        {
            string buildDir = AddressablesUtility.GetAddressablesRemoteBuildDir();

            if (Directory.Exists(buildDir))
            {
                Directory.Delete(buildDir, true);
            }
        }
    }
}
