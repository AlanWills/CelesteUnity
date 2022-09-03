using Celeste.Objects;
using CelesteEditor.BuildSystem.Steps;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Data
{
    [CreateAssetMenu(fileName = nameof(BuildPostProcessSteps), menuName = "Celeste/Build System/Build Post Process Steps")]
    public class BuildPostProcessSteps : ListScriptableObject<BuildPostProcessStep>
    {
    }
}
