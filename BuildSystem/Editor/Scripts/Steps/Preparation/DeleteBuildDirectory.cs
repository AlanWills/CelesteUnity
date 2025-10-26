using System;
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
                try
                {
                    Directory.Delete(platformSettings.BuildDirectory, true);
                }
                catch (Exception e)
                {
                   Debug.LogWarning($"Failed to delete build directory {platformSettings.BuildDirectory} due to exception: {e}.  " +
                                    $"This often occurs, but can be resolved by attempting a second delete which will be done programmatically.  Look for an error in the logs if this second attempt fails.");
                }
            }
            
            // Attempt to delete it again as this often resolves most exceptions
            if (Directory.Exists(platformSettings.BuildDirectory))
            {
                try
                {
                    Directory.Delete(platformSettings.BuildDirectory, true);
                }
                catch (Exception e)
                {
                   Debug.LogError($"Failed to delete build directory {platformSettings.BuildDirectory} for the second time due to exception: {e}.  " + 
                                  "This indicates a more unknown error - is a program using it?.");
                }
            }
        }
    }
}
