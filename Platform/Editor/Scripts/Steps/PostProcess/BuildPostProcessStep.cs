using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    public abstract class BuildPostProcessStep : ScriptableObject
    {
        public abstract void Execute(BuildPlayerOptions buildPlayerOptions, BuildReport result, PlatformSettings platformSettings);
    }
}