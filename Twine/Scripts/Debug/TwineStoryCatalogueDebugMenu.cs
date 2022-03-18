using Celeste.Debug.Menus;
using Celeste.Events;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace Celeste.Twine.Debug
{
    [CreateAssetMenu(fileName = nameof(TwineStoryCatalogueDebugMenu), menuName = "Celeste/Twine/Debug/Twine Story Catalogue Debug Menu")]
    public class TwineStoryCatalogueDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private TwineStoryCatalogue twineStoryCatalogue;
        [SerializeField] private TwineStoryEvent loadTwineStoryFromCatalogue;

        #endregion

        #region GUI

        protected override void OnDrawMenu()
        {
            for (int i = 0, n = twineStoryCatalogue.NumItems; i < n; ++i)
            {
                using (var horizontal = new HorizontalScope())
                {
                    TwineStory twineStory = twineStoryCatalogue.GetItem(i);

                    Label(twineStory.name);

                    if (Button($"Load", ExpandWidth(false)))
                    {
                        loadTwineStoryFromCatalogue.Invoke(twineStory);
                    }
                }
            }
        }

        #endregion
    }
}