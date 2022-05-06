using Celeste.Assets;
using System.Collections;
using UnityEngine;

namespace Celeste.Localisation.Settings
{
    public abstract class LocalisationSettings : ScriptableObject, IHasAssets
    {
        public abstract bool ShouldLoadAssets();
        public abstract IEnumerator LoadAssets();

        public abstract void SetCurrentLanguage(string languageCode);
        public abstract void SetCurrentLanguage(Language language);
    }
}
