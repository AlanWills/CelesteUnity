using Celeste.Debug.Menus;
using Celeste.Log;
using System.IO;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace Celeste.Twine.Debug
{
    [CreateAssetMenu(fileName = nameof(TwinePersistenceDebugMenu), menuName = "Celeste/Twine/Debug/Twine Persistence Debug Menu")]
    public class TwinePersistenceDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private TwineRecord twineRecord;
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
#if CELESTE_NATIVE_FILE_PICKER
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
                            twineRecord.ImportTwineStory(path);
                        }
                    }, new string[] { fileType });

                    HudLog.LogInfo($"Permission result: {permission}");
#else
                    UnityEngine.Debug.LogAssertion("Cannot import files without Celeste Native File Picker.");
#endif
                }

                if (Button($"Save Current", ExpandWidth(false)))
                {
                    saveCurrentTwineStory.Invoke();
                }
            }

            for (int i = 0, n = twineRecord.NumTwineStoryRecords; i < n; ++i)
            {
                using (var horizontal = new HorizontalScope())
                {
                    string twineStoryName = twineRecord.GetTwineStoryName(i);

                    Label(twineStoryName);

                    if (Button($"Load", ExpandWidth(false)))
                    {
                        twineRecord.LoadTwineStory(i);
                    }

                    if (Button($"Share", ExpandWidth(false)))
                    {
#if CELESTE_NATIVE_SHARE
                        string twineStoryPath = twineRecord.GetTwineStoryPath(i);
                        new NativeShare()
                            .AddFile(Path.Combine(Application.persistentDataPath, twineStoryPath))
                            .SetSubject($"Share {twineStoryName}")
                            .SetCallback((result, shareTarget) => HudLog.LogInfo($"Share result: {result}, selected app: {shareTarget}"))
                            .Share();
#else
                        UnityEngine.Debug.LogAssertion("Cannot share files with Celeste Native Share.");
#endif
                    }
                }
            }

            using (var horizontal = new HorizontalScope())
            {
                newStoryName = TextField(newStoryName);

                if (Button("Create", ExpandWidth(false)))
                {
                    twineRecord.CreateTwineStory(newStoryName);
                }
            }
        }

#endregion
    }
}