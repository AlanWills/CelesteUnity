using UnityEditor.AddressableAssets.Build;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    public abstract class AssetPostProcessStep : ScriptableObject
    {
        public abstract void Execute(AddressablesPlayerBuildResult result, PlatformSettings platformSettings);
    }
}