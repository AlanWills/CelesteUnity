using System;
using UnityEditor;

namespace CelesteEditor.Tools
{
    public class AssetEditingScope : IDisposable
    {
        public AssetEditingScope()
        {
            AssetDatabase.StartAssetEditing();
        }

        public void Dispose()
        {
            AssetDatabase.StopAssetEditing();
        }
    }
}