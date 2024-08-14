using Celeste.Debug.Menus;
using Celeste.Events;
using Celeste.Localisation.Catalogue;
using UnityEngine;

namespace Celeste.Localisation.Debug
{
    [CreateAssetMenu(fileName = nameof(LocalisationDebugMenu), menuName = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM + "Localisation Debug Menu", order = CelesteMenuItemConstants.LOCALISATION_MENU_ITEM_PRIORITY)]
    public class LocalisationDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private LanguageCatalogue languageCatalogue;
        [SerializeField] private LanguageEvent setCurrentLanguage;

        #endregion

        protected override void OnDrawMenu()
        {
            for (int i = 0, n = languageCatalogue.NumItems; i < n; ++i)
            {
                using (var horizontal = new GUILayout.HorizontalScope())
                {
                    Language language = languageCatalogue.GetItem(i);
                    GUILayout.Label(language.CountryCode);
                    
                    if (GUILayout.Button("Set", GUILayout.ExpandWidth(false)))
                    {
                        setCurrentLanguage.Invoke(language);
                    }
                }
            }
        }
    }
}