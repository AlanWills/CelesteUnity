using System.Collections;
using UnityEngine;

namespace Celeste.Localisation
{
    [CreateAssetMenu(fileName = nameof(Verb), menuName = "Celeste/Localisation/Verb")]
    public class Verb : ScriptableObject
    {
        #region Properties and Fields

        [SerializeField] private LocalisationKey infinitive;

        [Header("Present")]
        [SerializeField] private LocalisationKey presentILocalisationKey;
        [SerializeField] private LocalisationKey presentYouLocalisationKey;
        [SerializeField] private LocalisationKey presentHeLocalisationKey;
        [SerializeField] private LocalisationKey presentSheLocalisationKey;
        [SerializeField] private LocalisationKey presentWeLocalisationKey;
        [SerializeField] private LocalisationKey presentTheyLocalisationKey;

        [Header("Perfect")]
        [SerializeField] private LocalisationKey perfectILocalisationKey;
        [SerializeField] private LocalisationKey perfectYouLocalisationKey;
        [SerializeField] private LocalisationKey perfectHeLocalisationKey;
        [SerializeField] private LocalisationKey perfectSheLocalisationKey;
        [SerializeField] private LocalisationKey perfectWeLocalisationKey;
        [SerializeField] private LocalisationKey perfectTheyLocalisationKey;

        #endregion
    }
}