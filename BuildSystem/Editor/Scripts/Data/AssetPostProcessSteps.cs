using Celeste.Objects;
using CelesteEditor.BuildSystem.Steps;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Data
{
    [CreateAssetMenu(fileName = nameof(AssetPostProcessSteps), menuName = "Celeste/Build System/Asset Post Process Steps")]
    public class AssetPostProcessSteps : ListScriptableObject<AssetPostProcessStep>
    {
    }
}
