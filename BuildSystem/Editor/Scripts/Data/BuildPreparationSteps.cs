using Celeste.Objects;
using CelesteEditor.BuildSystem.Steps;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Data
{
    [CreateAssetMenu(fileName = nameof(BuildPreparationSteps), menuName = "Celeste/Build System/Build Preparation Steps")]
    public class BuildPreparationSteps : ListScriptableObject<BuildPreparationStep>
    {
    }
}
