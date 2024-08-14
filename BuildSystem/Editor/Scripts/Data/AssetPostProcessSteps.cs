#if USE_ADDRESSABLES
using Celeste;
using Celeste.Objects;
using CelesteEditor.BuildSystem.Steps;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Data
{
    [CreateAssetMenu(
        fileName = nameof(AssetPostProcessSteps), 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "Asset Post Process Steps",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class AssetPostProcessSteps : ListScriptableObject<AssetPostProcessStep>
    {
    }
}
#endif