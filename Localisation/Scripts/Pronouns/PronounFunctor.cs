using UnityEngine;

namespace Celeste.Localisation.Pronouns
{
    public abstract class PronounFunctor : ScriptableObject
    {
        public abstract string RemoveDefinitePronouns(string localisedText);
        public abstract string RemoveIndefinitePronouns(string localisedText);
    }
}
