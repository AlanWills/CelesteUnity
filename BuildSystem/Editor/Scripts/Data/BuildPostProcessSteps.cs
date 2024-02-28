using Celeste;
using Celeste.Objects;
using CelesteEditor.BuildSystem.Steps;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Data
{
    [CreateAssetMenu(
        fileName = nameof(BuildPostProcessSteps), 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "Build Post Process Steps",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class BuildPostProcessSteps : ListScriptableObject<BuildPostProcessStep>
    {
    }
}
