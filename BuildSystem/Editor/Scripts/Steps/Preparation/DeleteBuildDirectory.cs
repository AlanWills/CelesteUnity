using Celeste;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    [CreateAssetMenu(
        fileName = nameof(DeleteBuildDirectory), 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "Build Preparation/Delete Build Directory",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class DeleteBuildDirectory : BuildPreparationStep
    {
        public override void Execute(BuildPlayerOptions buildPlayerOptions, PlatformSettings platformSettings)
        {
            if (Directory.Exists(platformSettings.BuildDirectory))
            {
                Directory.Delete(platformSettings.BuildDirectory, true);
            }
        }
    }
}
