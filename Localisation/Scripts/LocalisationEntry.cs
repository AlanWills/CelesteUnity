using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Localisation
{
    [CreateAssetMenu(fileName = "LocalisationEntry", menuName = "Celeste/Localisation/Localisation Entry")]
    public class LocalisationEntry : ScriptableObject
    {
        public string key;
        [TextArea] public string text;
    }
}
