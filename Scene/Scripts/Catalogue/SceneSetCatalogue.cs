using Celeste.Objects;
using UnityEngine;

namespace Celeste.Scene.Catalogue
{
    [CreateAssetMenu(fileName = nameof(SceneSetCatalogue), order = CelesteMenuItemConstants.SCENE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SCENE_MENU_ITEM + "Scene Set Catalogue")]
    public class SceneSetCatalogue : ListScriptableObject<SceneSet>, IAutomaticImportSettings
    {
        public AutomaticImportBehaviour ImportBehaviour => importBehaviour;

        [SerializeField] private AutomaticImportBehaviour importBehaviour = AutomaticImportBehaviour.ImportAllAssets;
    }
}
