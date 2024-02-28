using Celeste;
using Celeste.Objects;
using CelesteEditor.BuildSystem.Steps;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Data
{
    [CreateAssetMenu(
        fileName = nameof(AssetPreparationSteps), 
        menuName = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM + "Asset Preparation Steps",
        order = CelesteMenuItemConstants.BUILDSYSTEM_MENU_ITEM_PRIORITY)]
    public class AssetPreparationSteps : ListScriptableObject<AssetPreparationStep>
    {
    }
}
