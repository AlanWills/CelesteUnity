using UnityEngine;

namespace Celeste.Localisation
{
    [CreateAssetMenu(fileName = nameof(LocalisationKey), menuName = "Celeste/Localisation/Localisation Key")]
    public class LocalisationKey : ScriptableObject
    {
        #region Properties and Fields

        public string Key => key;
        public string Fallback => fallback;

        [SerializeField] private string key;
        [SerializeField, TextArea] private string fallback;

        #endregion
    }
}
