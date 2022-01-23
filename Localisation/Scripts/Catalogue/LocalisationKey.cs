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
            set 
            { 
                key = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public string Fallback
        {
            get { return fallback; }
            set 
            { 
                fallback = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public LocalisationKeyCategory Category
        {
            get { return category; }
            set
            {
                category = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        [SerializeField] private string key;
        [SerializeField, TextArea] private string fallback;
        [SerializeField] private LocalisationKeyCategory category;

        #endregion

        public override string ToString()
        {
            return key;
        }
    }
}
