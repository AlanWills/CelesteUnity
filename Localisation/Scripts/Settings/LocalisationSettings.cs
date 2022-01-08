#if UNITY_EDITOR
using Celeste.Localisation.Parameters;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Celeste.Localisation.Settings
{
    [FilePath("Assets/Celeste/Localisation/Editor/Data/LocalisationSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    [CreateAssetMenu(fileName = nameof(LocalisationSettings), menuName = "Celeste/Localisation/Localisation Settings")]
    public class LocalisationSettings : ScriptableSingleton<LocalisationSettings>
    {
        public LanguageValue currentLanguageValue;
    }
}
#endif