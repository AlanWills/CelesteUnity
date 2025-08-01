﻿using Celeste.Objects;
using UnityEngine;

namespace Celeste.Narrative
{
    [CreateAssetMenu(fileName = nameof(ChapterCatalogue), menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Production/Chapter Catalogue", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class ChapterCatalogue : ListScriptableObject<Chapter>, IAutomaticImportSettings
    {
        public AutomaticImportBehaviour ImportBehaviour => importBehaviour;

        [SerializeField] private AutomaticImportBehaviour importBehaviour = AutomaticImportBehaviour.ImportAssetsInCatalogueDirectoryAndSubDirectories;
        
        public Chapter FindByGuid(int guid)
        {
            return FindItem(x => x.Guid == guid);
        }
    }
}