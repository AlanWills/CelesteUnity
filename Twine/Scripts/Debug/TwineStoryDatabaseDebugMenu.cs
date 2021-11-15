using Celeste.Debug.Menus;
using Celeste.Events;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace Celeste.Twine.Debug
{
    [CreateAssetMenu(fileName = nameof(TwineStoryDatabaseDebugMenu), menuName = "Celeste/Twine/Debug/Twine Story Database Debug Menu")]
    public class TwineStoryDatabaseDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private TwineStoryDatabase twineStoryDatabase;
        [SerializeField] private TwineStoryEvent loadTwineStoryFromDatabase;

        #endregion

        #region GUI

        protected override void OnDrawMenu()
        {
            for (int i = 0, n = twineStoryDatabase.NumItems; i < n; ++i)
            {
                using (var horizontal = new HorizontalScope())
                {
                    TwineStory twineStory = twineStoryDatabase.GetItem(i);

                    Label(twineStory.name);

                    if (Button($"Load", ExpandWidth(false)))
                    {
                        loadTwineStoryFromDatabase.Invoke(twineStory);
                    }
                }
            }
        }

        #endregion
    }
}