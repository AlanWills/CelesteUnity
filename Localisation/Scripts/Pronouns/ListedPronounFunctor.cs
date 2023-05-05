using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Localisation.Pronouns
{
    [CreateAssetMenu(fileName = nameof(ListedPronounFunctor), menuName = "Celeste/Localisation/Pronouns/Listed Pronoun Functor")]
    public class ListedPronounFunctor : PronounFunctor
    {
        #region Properties and Fields

        [SerializeField] private List<string> definitePronouns = new List<string>();
        [SerializeField] private List<string> indefinitePronouns = new List<string>();

        #endregion

        public override string RemoveDefinitePronouns(string localisedText)
        {
            return StripPronouns(localisedText, definitePronouns);
        }

        public override string RemoveIndefinitePronouns(string localisedText)
        {
            return StripPronouns(localisedText, indefinitePronouns);
        }

        private static string StripPronouns(string localisedText, IReadOnlyList<string> pronouns)
        {
            foreach (string pronoun in pronouns)
            {
                // Check if the text starts with this pronoun and a space
                if (localisedText.Length > (pronoun.Length + 1) &&
                    localisedText.StartsWith(pronoun) &&
                    localisedText[pronoun.Length] == ' ')
                {
                    return localisedText.Substring(pronoun.Length + 1);
                }
            }

            return localisedText;
        }
    }
}
