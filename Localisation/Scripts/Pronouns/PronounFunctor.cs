using Celeste.OdinSerializer;

namespace Celeste.Localisation.Pronouns
{
    public abstract class PronounFunctor : SerializedScriptableObject
    {
        public abstract string RemoveDefinitePronouns(string localisedText);
        public abstract string RemoveIndefinitePronouns(string localisedText);
    }
}
