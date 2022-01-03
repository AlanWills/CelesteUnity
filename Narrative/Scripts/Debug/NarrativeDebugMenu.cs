using Celeste.Debug.Menus;
using Celeste.Narrative.Loading;
using Celeste.Scene.Events;
using UnityEngine;

namespace Celeste.Narrative.Debug
{
    [CreateAssetMenu(fileName = nameof(NarrativeDebugMenu), menuName = "Celeste/Narrative/Debug/Narrative Debug Menu")]
    public class NarrativeDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private StoryCatalogue storyCatalogue;
        [SerializeField] private NarrativeRecord narrativeRecord;
        [SerializeField] private OnContextLoadedEvent onContextLoadedEvent;
        [SerializeField] private Celeste.Events.Event saveNarrativeRecord;

        #endregion

        #region GUI

        protected override void OnDrawMenu()
        {
            using (var horizontal = new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Load Last"))
                {
                    ChapterRecord lastPlayedChapter = narrativeRecord.FindLastPlayedChapterRecord(storyCatalogue);
                    UnityEngine.Debug.Assert(lastPlayedChapter != null, $"Could not find last played chapter.");

                    if (lastPlayedChapter != null)
                    {
                        NarrativeContext narrativeContext = new NarrativeContext(lastPlayedChapter);
                        onContextLoadedEvent.Invoke(new OnContextLoadedArgs(narrativeContext));
                    }
                }

                RuntimePlatform platform = Application.platform;
                bool isWindows = platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsEditor;

                if (isWindows && GUILayout.Button("Open Save"))
                {
                    System.Diagnostics.Process.Start("explorer.exe", Application.persistentDataPath.Replace('/', '\\'));
                }
            }

            GUILayout.Space(10);

            for (int storyRecordIndex = 0, numStoryRecords = narrativeRecord.NumStoryRecords; storyRecordIndex < numStoryRecords; ++storyRecordIndex)
            {
                StoryRecord storyRecord = narrativeRecord.GetStoryRecord(storyRecordIndex);
                GUILayout.Label(storyRecord.StoryName);

                for (int chapterRecordIndex = 0, numChapterRecords = storyRecord.NumChapterRecords; chapterRecordIndex < numChapterRecords; ++chapterRecordIndex)
                {
                    ChapterRecord chapterRecord = storyRecord.GetChapterRecord(chapterRecordIndex);

                    using (var horizontal = new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label(chapterRecord.ChapterName);

                        if (chapterRecord.Progress < 1 && GUILayout.Button("Complete", GUILayout.ExpandWidth(false)))
                        {
                            chapterRecord.Complete();
                            saveNarrativeRecord.Invoke();
                        }

                        if (chapterRecord.Progress > 0 && GUILayout.Button("Reset", GUILayout.ExpandWidth(false)))
                        {
                            chapterRecord.ResetProgress();
                            saveNarrativeRecord.Invoke();
                        }
                    }
                }
            }
        }

        #endregion
    }
}