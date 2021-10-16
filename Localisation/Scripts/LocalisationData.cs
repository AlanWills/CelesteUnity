using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Localisation
{
    [CreateAssetMenu(fileName = "LocalisationData", menuName = "Celeste/Localisation/Localisation Data")]
    public class LocalisationData : ScriptableObject
    {
        #region Properties and Fields

        [SerializeField] private List<LocalisationEntry> localisationEntries = new List<LocalisationEntry>();

        #endregion
    }
}
