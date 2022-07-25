using Celeste.OdinSerializer;

namespace Celeste.Localisation
{
    public abstract class NumberToLocalisedTextConverter : SerializedScriptableObject
    {
        public abstract string Localise(int number, Language targetLanguage);
    }
}
