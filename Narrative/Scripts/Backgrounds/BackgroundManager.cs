using Celeste.Assets;
using Celeste.Narrative.Backgrounds.Settings;
using System.Collections;
using Celeste.Narrative.Requests;
using Celeste.Requests;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Narrative.Backgrounds
{
    [AddComponentMenu("Celeste/Narrative/Backgrounds/Background Manager")]
    public class BackgroundManager : MonoBehaviour, IHasAssets
    {
        #region Properties and Fields

        [SerializeField] private RectTransform backgroundRectTransform;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private AspectRatioFitter backgroundRatioFitter;
        [SerializeField] private BackgroundSettings backgroundSettings;

        private Coroutine backgroundAnimationCoroutine;

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

        private IEnumerator AnimateBackground(AnimateBackgroundRequestArgs args, SuccessCallback successCallback)
        {
            Vector2 startingPosition = backgroundRectTransform.anchoredPosition;
            float width = backgroundRectTransform.rect.width / backgroundRatioFitter.aspectRatio;
            float finishingPosition = -width * args.FinishOffset;
            
            float currentTime = 0;
            while (currentTime < args.AnimationTime)
            {
                yield return null;
                
                currentTime += Time.deltaTime;

                float lerpTime = args.UseAnimCurve ? args.AnimationCurve.Evaluate(currentTime) : currentTime / args.AnimationTime;
                float currentX = Mathf.Lerp(startingPosition.x, finishingPosition, lerpTime);
                backgroundRectTransform.anchoredPosition = new Vector2(currentX, startingPosition.y);   
            }
            
            backgroundRectTransform.anchoredPosition = new Vector2(finishingPosition, startingPosition.y);

            backgroundAnimationCoroutine = null;
            successCallback();
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

                Vector2 currentAnchoredPosition = backgroundRectTransform.anchoredPosition;
                backgroundRectTransform.anchoredPosition = new Vector2(0, currentAnchoredPosition.y);
            }
            else
            {
                backgroundImage.enabled = false;
            }
        }

        public void OnSetBackgroundRequest(AnimateBackgroundRequestArgs animateBackgroundRequestArgs, SuccessCallback successCallback, FailureCallback failureCallback)
        {
            if (!backgroundImage.enabled)
            {
                failureCallback.Invoke(AnimateBackgroundRequest.NO_BACKGROUND_SET_ERROR_CODE, 
                    "There is no background currently set, yet you're trying to animate a background.  Have you forgotten to wait for a previous request to be completed?");
                return;
            }
            
            if (backgroundAnimationCoroutine != null)
            {
                failureCallback.Invoke(AnimateBackgroundRequest.BACKGROUND_ANIMATION_ALREADY_IN_PROGRESS_ERROR_CODE, 
                    "There is already a background animation in progress whilst trying to set a new background.  Have you forgotten to wait for a previous request to be completed?");
                return;
            }
            
            backgroundAnimationCoroutine = StartCoroutine(AnimateBackground(animateBackgroundRequestArgs, successCallback));
        }

        #endregion
    }
}