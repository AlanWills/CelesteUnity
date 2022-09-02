using UnityEngine;

namespace CelesteEditor.BuildSystem.Steps
{
    public abstract class AssetPreparationStep : ScriptableObject
    {
        public abstract void Execute();
    }
}