using Celeste.Localisation;
using System.Collections.Generic;

namespace CelesteEditor.Localisation.Utility
{
    public static class LocalisationUtility
    {
        public static string ToAssetName(this string key)
        {
            List<char> characters = new List<char>(key.ToLower().ToCharArray());
            characters[0] = char.ToUpper(characters[0]);

            int delimiterIndex = characters.IndexOf('_');
            while (0 <= delimiterIndex)
            {
                characters.RemoveAt(delimiterIndex);
                characters[delimiterIndex] = char.ToUpper(characters[delimiterIndex]);
                delimiterIndex = characters.IndexOf('_', delimiterIndex);
            }

            return new string(characters.ToArray());
        }
    }
}