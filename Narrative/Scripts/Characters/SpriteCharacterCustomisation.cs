using UnityEditor;
using UnityEngine;

namespace Celeste.Narrative.Characters
{
    [CreateAssetMenu(fileName = nameof(SpriteCharacterCustomisation), menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Characters/Sprite Character Customisation", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class SpriteCharacterCustomisation : CharacterCustomisation
    {
        public Sprite Sprite
        {
            get { return sprite; }
        }

        [SerializeField] private Sprite sprite;
    }
}