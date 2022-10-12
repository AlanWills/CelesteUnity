using System;

namespace Celeste.BuildSystem
{
    [Serializable]
    public struct RuntimeBuildSettings
    {
        public string RemoteContentCatalogueHashURL;
        public string RemoteContentCatalogueJsonURL;
    }
}