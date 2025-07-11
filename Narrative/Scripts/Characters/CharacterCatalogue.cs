using Celeste.Objects;
using UnityEditor;
using UnityEngine;

namespace Celeste.Narrative.Characters
{
    [CreateAssetMenu(fileName = nameof(CharacterCatalogue), menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Characters/Character Catalogue", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class CharacterCatalogue : ListScriptableObject<Character>, IAutomaticImportSettings
    {
        #region Properties and Fields

        public AutomaticImportBehaviour ImportBehaviour => importBehaviour;
        
        [SerializeField] private AutomaticImportBehaviour importBehaviour;
        
        #endregion
        
        public Character FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}