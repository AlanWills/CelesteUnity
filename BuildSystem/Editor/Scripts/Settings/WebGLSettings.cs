using Celeste;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BuildSystem
{
    [CreateAssetMenu(
        fileName = nameof(WebGLSettings), 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "WebGL Settings",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class WebGLSettings : PlatformSettings
    {
        protected override void SetPlatformDefaultValues(bool isDebugConfig)
        {
            BuildTarget = BuildTarget.WebGL;
            BuildTargetGroup = BuildTargetGroup.WebGL;
        }

        protected override void ApplyImpl()
        {
        }
    }
}
