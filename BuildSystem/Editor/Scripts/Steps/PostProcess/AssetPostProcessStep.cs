using UnityEditor.AddressableAssets.Build;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    public abstract class AssetPostProcessStep : ScriptableObject
    {
        #region Properties and Fields

        public bool OnlyExecuteOnSuccess => onlyExecuteOnSuccess;

        [SerializeField] private bool onlyExecuteOnSuccess;

        #endregion

        public abstract void Execute(AddressablesPlayerBuildResult result, PlatformSettings platformSettings);
    }
}