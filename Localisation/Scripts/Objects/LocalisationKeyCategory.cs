using System.Collections;
using UnityEngine;

namespace Celeste.Localisation
{
    [CreateAssetMenu(fileName = nameof(LocalisationKeyCategory), menuName = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM + "Localisation Key Category", order = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM_PRIORITY)]
    public class LocalisationKeyCategory : ScriptableObject
    {
        #region Properties and Fields

        public string CategoryName
        {
            get { return categoryName; }
            set
            {
                categoryName = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        [SerializeField] private string categoryName;

        #endregion
    }
}