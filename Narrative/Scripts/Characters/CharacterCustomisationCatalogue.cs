using Celeste.Objects;
using UnityEditor;
using UnityEngine;

namespace Celeste.Narrative.Characters
{
    [CreateAssetMenu(fileName = nameof(CharacterCustomisation), menuName = "Celeste/Narrative/Characters/Character Customisation Catalogue")]
    public class CharacterCustomisationCatalogue : ArrayScriptableObject<CharacterCustomisation>
    {
        public T FindByGuid<T>(int guid) where T : CharacterCustomisation
        {
            return FindItem(x => x is T && x.Guid == guid) as T;
        }
    }
}