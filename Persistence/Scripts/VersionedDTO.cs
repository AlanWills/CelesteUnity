using System;

namespace Celeste.Persistence
{
    [Serializable]
    public class VersionedDTO : IVersioned
    {
        public VersionInformation versionInformation;

        #region IVersioned

        int IVersioned.Version { get => versionInformation.Version; set => versionInformation.Version = value; }
        DateTimeOffset IVersioned.SaveTime { get => versionInformation.SaveTime; set => versionInformation.SaveTime = value; }

        bool IVersioned.IsLowerVersionThan(IVersioned version)
        {
            return versionInformation.IsLowerVersionThan(version);
        }

        #endregion
    }
}