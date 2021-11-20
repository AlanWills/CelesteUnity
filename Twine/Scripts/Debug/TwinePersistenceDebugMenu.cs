using Celeste.Debug.Menus;
using Celeste.Events;
using Celeste.Log;
using Celeste.Twine.Persistence;
using System.IO;
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
            using (var horizontal = new HorizontalScope())
            {
                if (Button($"Import", ExpandWidth(false)))
                {
                    string fileType = NativeFilePicker.ConvertExtensionToFileType(TwineStory.FILE_EXTENSION);
                    NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
                    {
                        if (string.IsNullOrWhiteSpace(path))
                        {
                            HudLog.LogInfo("Operation cancelled");
                        }
                        else
                        {
                            HudLog.LogInfo($"Picked file: {path}");
                            TwinePersistence.Instance.ImportTwineStory(path);
                        }
                    }, new string[] { fileType });

                    HudLog.LogInfo($"Permission result: {permission}");
                }

                if (Button($"Save Current", ExpandWidth(false)))
                {
                    saveCurrentTwineStory.Invoke();
                }
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

                    if (Button($"Share", ExpandWidth(false)))
                    {
                        string twineStoryPath = TwinePersistence.Instance.GetTwineStoryPath(i);
                        new NativeShare()
                            .AddFile(twineStoryPath)
                            .SetSubject($"Share {twineStoryName}")
                            .SetCallback((result, shareTarget) => HudLog.LogInfo($"Share result: {result}, selected app: {shareTarget}"))
                            .Share();
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