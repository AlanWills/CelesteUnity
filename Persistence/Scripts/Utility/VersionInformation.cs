using System;

namespace Celeste.Persistence
{
    [Serializable]
    public struct VersionInformation : IVersioned
    {
        int IVersioned.Version { get => Version; set => Version = value; }
        DateTimeOffset IVersioned.SaveTime { get => SaveTime; set => SaveTime = value; }

        public int Version;
        public DateTimeOffset SaveTime;

        public bool IsLowerVersionThan(IVersioned version)
        {
            if (version == null)
            {
                // We can never be strictly lower version than null, it's not possible
                return false;
            }

            bool isVersionLower = Version < version.Version;
            bool isSaveTimeOlder = SaveTime < version.SaveTime;
            return isVersionLower || isSaveTimeOlder;
        }
    }
}