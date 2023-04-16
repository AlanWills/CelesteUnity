using Celeste.OdinSerializer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Localisation
{
    public abstract class NumberToLocalisedTextConverter : SerializedScriptableObject
    {
        #region Properties and Fields

        [SerializeField] private List<LocalisationKey> numericalSuffixes = new List<LocalisationKey>();

        #endregion

        public virtual string Truncate(int number, Language targetLanguage)
        {
            int absValue = Math.Abs(number);

            int logBase1000 = ((int)Mathf.Log10(absValue)) / 3;
            if (logBase1000 < 1 || logBase1000 >= numericalSuffixes.Count)
            {
                // Our number is less than a thousand or larger than our handled numbers so we have no suffix
                return number.ToString();
            }

            int bestThousandMultiple = (int)Mathf.Pow(1000, logBase1000);
            int remainder = number / bestThousandMultiple;
            return $"{remainder}{targetLanguage.Localise(numericalSuffixes[logBase1000 - 1])}";
        }

        public abstract string Localise(int number, Language targetLanguage);
    }
}
