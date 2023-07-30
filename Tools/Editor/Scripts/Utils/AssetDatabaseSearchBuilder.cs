using System;

namespace CelesteEditor.Tools.Utils
{
    public class AssetDatabaseSearchBuilder
    {
        #region Properties and Fields

        private string assetName;
        private string typeName;

        #endregion

        public AssetDatabaseSearchBuilder WithAssetName(string assetName)
        {
            this.assetName = assetName;
            return this;
        }

        public AssetDatabaseSearchBuilder WithType<T>() where T : UnityEngine.Object
        {
            return WithType(typeof(T));
        }

        public AssetDatabaseSearchBuilder WithType(Type t)
        {
            typeName = t.Name;
            return this;
        }

        public string Build()
        {
            return $"{assetName} t:{typeName}".Trim();
        }
    }
}