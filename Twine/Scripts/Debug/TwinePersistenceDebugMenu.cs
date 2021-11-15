using Celeste.Debug.Menus;
using Celeste.Events;
using Celeste.Twine.Persistence;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace Celeste.Twine.Debug
{
    [CreateAssetMenu(fileName = nameof(TwinePersistenceDebugMenu), menuName = "Celeste/Twine/Debug/Twine Persistence Debug Menu")]
    public class TwinePersistenceDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private TwineStoryEvent loadTwineStoryFromPersistence;
        [SerializeField] private TwineStoryEvent createTwineStory;
        [SerializeField] private Events.Event saveCurrentTwineStory;

        private string newStoryName = "NewStory";

        #endregion

        #region GUI

        protected override void OnDrawMenu()
        {
            if (Button($"Save Current", ExpandWidth(false)))
            {
                saveCurrentTwineStory.Invoke();
            }

            for (int i = 0, n = TwinePersistence.Instance.NumTwineStories; i < n; ++i)
            {
                using (var horizontal = new HorizontalScope())
                {
                    string twineStoryName = TwinePersistence.Instance.GetTwineStoryName(i);

                    Label(twineStoryName);

                    if (Button($"Load", ExpandWidth(false)))
                    {
                        TwineStory twineStory = TwinePersistence.Instance.LoadTwineStory(i);
                        if (twineStory != null)
                        {
                            loadTwineStoryFromPersistence.Invoke(twineStory);
                        }
                    }
                }
            }

            using (var horizontal = new HorizontalScope())
            {
                newStoryName = TextField(newStoryName);

                if (Button("Create", ExpandWidth(false)))
                {
                    TwineStory newTwineStory = TwinePersistence.Instance.AddTwineStory(newStoryName);
                    if (newTwineStory != null)
                    {
                        createTwineStory.Invoke(newTwineStory);
                    }
                }
            }
        }

        #endregion
    }
}