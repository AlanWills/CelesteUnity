using Celeste.Objects;
using CelesteEditor.Platform.Steps;
using UnityEngine;

namespace CelesteEditor.Platform.Data
{
    [CreateAssetMenu(fileName = nameof(AssetPreparationSteps), menuName = "Celeste/Platform/Asset Preparation Steps")]
    public class AssetPreparationSteps : ListScriptableObject<AssetPreparationStep>
    {
    }
}
