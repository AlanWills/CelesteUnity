using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Platform.Steps
{
    public abstract class AssetPreparationStep : ScriptableObject
    {
        public abstract void Execute();
    }
}