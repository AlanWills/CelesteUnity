using Celeste.Objects;
using UnityEditor;

namespace CelesteEditor.Objects
{
    public class IInitializableAssetPostProcessor : AssetPostprocessor
    {
        private void OnPreprocessAsset()
        {
            if (context.mainObject is IInitializable)
            {
                (context.mainObject as IInitializable).Initialize();
            }
        }
    }
}
