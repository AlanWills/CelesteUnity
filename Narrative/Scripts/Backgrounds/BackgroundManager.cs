using Celeste.Assets;
using Celeste.Narrative.Backgrounds.Settings;
using System.Collections;
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

        private void OnSetBackground(Background background)
        {
            if (background != null && background.Sprite != null)
            {
                backgroundSettings.CurrentBackgroundGuid = background.Guid;
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

        public void OnNarrativeBegin(NarrativeRuntime narrativeRuntime)
        {
            Background background = backgroundSettings.FindCurrentBackground();

            if (background != null)
            {
                OnSetBackground(background);
            }
        }

        #endregion
    }
}