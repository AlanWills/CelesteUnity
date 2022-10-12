using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    public abstract class BuildPreparationStep : ScriptableObject
    {
        public abstract void Execute(BuildPlayerOptions buildPlayerOptions, PlatformSettings platformSettings);
    }
}