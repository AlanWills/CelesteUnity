using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    public abstract class BuildPostProcessStep : ScriptableObject
    {
        #region Properties and Fields

        public bool OnlyExecuteOnSuccess => onlyExecuteOnSuccess;

        [SerializeField] private bool onlyExecuteOnSuccess;

        #endregion

        public abstract void Execute(BuildPlayerOptions buildPlayerOptions, BuildReport result, PlatformSettings platformSettings);
    }
}