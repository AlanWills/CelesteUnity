using Celeste.Web;
using System.Collections.Generic;
using UnityEngine;

// Cannot be moved to the editor project - this is because it's needed by the LocalisationSettings
// which must live in this assembly to be used by components
namespace Celeste.Localisation.Tools
{
    public struct LocalisationPostImportContext
    {
        public GoogleSheet googleSheet;
        public Dictionary<string, LocalisationKey> localisationKeyLookup;
    }

    public abstract class LocalisationPostImportStep : ScriptableObject
    {
        public abstract void Execute(LocalisationPostImportContext localisationPostImportContext);
    }
}