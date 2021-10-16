using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Localisation
{
    [AddComponentMenu("DnD/Localisation/Localisation Manager")]
    public class LocalisationManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private LocalisationData localisationData;

        #endregion
    }
}
