using Celeste;
using Celeste.Objects;
using CelesteEditor.BuildSystem.Steps;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Data
{
    [CreateAssetMenu(
        fileName = nameof(BuildPreparationSteps), 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "Build Preparation Steps",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class BuildPreparationSteps : ListScriptableObject<BuildPreparationStep>
    {
    }
}
