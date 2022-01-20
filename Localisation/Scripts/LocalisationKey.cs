using UnityEngine;

namespace Celeste.Localisation
{
    [CreateAssetMenu(fileName = nameof(LocalisationKey), menuName = "Celeste/Localisation/Localisation Key")]
    public class LocalisationKey : ScriptableObject
    {
        #region Properties and Fields

        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        public string Fallback
        {
            get { return fallback; }
            set { fallback = value; }
        }

        [SerializeField] private string key;
        [SerializeField, TextArea] private string fallback;

        #endregion

        public override string ToString()
        {
            return key;
        }
    }
}
