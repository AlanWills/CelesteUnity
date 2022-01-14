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
        [SerializeField] private LocalisationKey presentHeSheItLocalisationKey;
        [SerializeField] private LocalisationKey presentWeLocalisationKey;
        [SerializeField] private LocalisationKey presentTheyLocalisationKey;

        #endregion
    }
}