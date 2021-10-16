using UnityEditor;
using UnityEngine;

namespace Celeste.Narrative.Characters
{
    [CreateAssetMenu(fileName = nameof(SpriteCharacterCustomisation), menuName = "Celeste/Narrative/Characters/Sprite Character Customisation")]
    public class SpriteCharacterCustomisation : CharacterCustomisation
    {
        public Sprite Sprite
        {
            get { return sprite; }
        }

        [SerializeField] private Sprite sprite;
    }
}