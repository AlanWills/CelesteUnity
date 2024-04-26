using Celeste.Objects;
using UnityEngine;

namespace Celeste.Logic.Catalogue
{
    [CreateAssetMenu(fileName = nameof(ConditionCatalogue), menuName = CelesteMenuItemConstants.LOGIC_MENU_ITEM + "Condition Catalogue", order = CelesteMenuItemConstants.LOGIC_MENU_ITEM_PRIORITY)]
    public class ConditionCatalogue : ListScriptableObject<Condition>, IAutomaticImportSettings
    {
        public AutomaticImportBehaviour ImportBehaviour => importBehaviour;

        [SerializeField] private AutomaticImportBehaviour importBehaviour = AutomaticImportBehaviour.ImportAssetsInCatalogueDirectoryAndSubDirectories;
    }
}
