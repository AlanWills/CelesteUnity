using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Localisation.Tools
{
    public struct LocalisationPostImportContext
    {
        public Dictionary<string, LocalisationKey> localisationKeyLookup;
    }

    public abstract class LocalisationPostImportStep : ScriptableObject
    {
        public abstract void Execute(LocalisationPostImportContext localisationPostImportContext);
    }
}