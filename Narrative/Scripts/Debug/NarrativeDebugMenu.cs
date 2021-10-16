using Celeste.Debug.Menus;
using Celeste.Narrative.Loading;
using Celeste.Narrative.Persistence;
using Celeste.Scene.Events;
using System.Collections;
using UnityEngine;

namespace Celeste.Narrative.Debug
{
    [CreateAssetMenu(fileName = nameof(NarrativeDebugMenu), menuName = "Celeste/Narrative/Debug/Narrative Debug Menu")]
    public class NarrativeDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private OnContextLoadedEvent onContextLoadedEvent;

        #endregion

        #region GUI

        protected override void OnDrawMenu()
        {
            if (GUILayout.Button("Load Last Played"))
            {
                ChapterRecord lastPlayedChapter = NarrativePersistence.Instance.FindLastPlayedChapterRecord();
                UnityEngine.Debug.Assert(lastPlayedChapter != null, $"Could not find last played chapter.");

                if (lastPlayedChapter != null)
                {
                    NarrativeContext narrativeContext = new NarrativeContext(lastPlayedChapter);
                    onContextLoadedEvent.Invoke(new OnContextLoadedArgs(narrativeContext));
                }
            }
        }

        #endregion
    }
}