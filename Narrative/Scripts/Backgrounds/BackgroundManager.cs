using Celeste.Narrative.Parameters;
using Celeste.Narrative.Persistence;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Narrative.Characters
{
    [AddComponentMenu("Celeste/Narrative/Backgrounds/Background Manager")]
    public class BackgroundManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private BackgroundCatalogue backgroundCatalogue;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private AspectRatioFitter backgroundRatioFitter;
        [SerializeField] private ChapterRecordValue currentChapterRecord;

        #endregion

        public void SetBackground(Background background)
        {
            if (background != null && background.Sprite != null)
            {
                currentChapterRecord.Value.CurrentBackgroundGuid = background.Guid;
                backgroundImage.sprite = background.Sprite;
                backgroundImage.enabled = true;
                backgroundRatioFitter.aspectRatio = background.AspectRatio;
            }
            else
            {
                backgroundImage.enabled = false;
                UnityEngine.Debug.LogAssertion($"Attempting to set a null background.");
            }
        }

        #region Callbacks

        public void OnNarrativeBegin(NarrativeRuntime narrativeRuntime)
        {
            ChapterRecord chapterRecord = narrativeRuntime.Record;
            Background background = backgroundCatalogue.FindByGuid(chapterRecord.CurrentBackgroundGuid);

            if (background != null)
            {
                SetBackground(background);
            }
        }

        #endregion
    }
}