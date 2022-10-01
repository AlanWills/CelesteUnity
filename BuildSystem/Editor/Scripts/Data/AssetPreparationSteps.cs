using Celeste.Objects;
using CelesteEditor.BuildSystem.Steps;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Data
{
    [CreateAssetMenu(fileName = nameof(AssetPreparationSteps), menuName = "Celeste/Build System/Asset Preparation Steps")]
    public class AssetPreparationSteps : ListScriptableObject<AssetPreparationStep>
    {
    }
}
