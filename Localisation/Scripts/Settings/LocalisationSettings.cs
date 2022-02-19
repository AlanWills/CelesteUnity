#if UNITY_EDITOR
using Celeste.Localisation.Parameters;
using Celeste.Localisation.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Celeste.Localisation.Settings
{
    [FilePath("Assets/Celeste/Localisation/Editor/Data/LocalisationSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    [CreateAssetMenu(fileName = nameof(LocalisationSettings), menuName = "Celeste/Localisation/Localisation Settings")]
    public class LocalisationSettings : ScriptableSingleton<LocalisationSettings>
    {
        public LanguageValue currentLanguageValue;
        public List<LocalisationPostImportStep> postImportSteps = new List<LocalisationPostImportStep>();
    }
}
#endif