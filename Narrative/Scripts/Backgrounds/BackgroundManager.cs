using Celeste.Assets;
using Celeste.Narrative.Backgrounds.Settings;
using System.Collections;
using Celeste.FSM;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Narrative.Backgrounds
{
    [AddComponentMenu("Celeste/Narrative/Backgrounds/Background Manager")]
    public class BackgroundManager : MonoBehaviour, IHasAssets
    {
        #region Properties and Fields

        [SerializeField] private Image backgroundImage;
        [SerializeField] private AspectRatioFitter backgroundRatioFitter;
        [SerializeField] private BackgroundSettings backgroundSettings;

        #endregion

        public bool ShouldLoadAssets()
        {
            return backgroundSettings.ShouldLoadAssets();
        }

        public IEnumerator LoadAssets()
        {
            yield return backgroundSettings.LoadAssets();

            backgroundSettings.AddSetBackgroundListener(OnSetBackground);
        }

        #region Callbacks

        public void OnNarrativeBegin(NarrativeRuntime narrativeRuntime)
        {
            Background background = backgroundSettings.FindBackgroundByGuid(narrativeRuntime.ChapterRecord.CurrentBackgroundGuid);
            OnSetBackground(background); 
        }

        private void OnSetBackground(Background background)
        {
            if (background != null && background.Sprite != null)
            {
                backgroundImage.sprite = background.Sprite;
                backgroundImage.enabled = true;
                backgroundRatioFitter.aspectRatio = background.AspectRatio;
            }
            else
            {
                backgroundImage.enabled = false;
            }
        }

        #endregion
    }
}