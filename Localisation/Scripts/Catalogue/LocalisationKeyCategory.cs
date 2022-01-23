using System.Collections;
using UnityEngine;

namespace Celeste.Localisation
{
    [CreateAssetMenu(fileName = nameof(LocalisationKeyCategory), menuName = "Celeste/Localisation/Localisation Key Category")]
    public class LocalisationKeyCategory : ScriptableObject
    {
        #region Properties and Fields

        public string CategoryName => categoryName;

        [SerializeField] private string categoryName;

#endregion
    }
}