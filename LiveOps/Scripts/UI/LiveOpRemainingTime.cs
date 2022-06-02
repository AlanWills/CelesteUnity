using Celeste.Components;
using Celeste.Core;
using Celeste.Localisation;
using Celeste.Localisation.Parameters;
using Celeste.Localisation.Settings;
using Celeste.Tools;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Celeste.LiveOps.UI
{
    [AddComponentMenu("Celeste/Live Ops/UI/Live Op Remaining Time")]
    public class LiveOpRemainingTime : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private LanguageValue currentLanguage;
        [SerializeField] private LocalisationKey outOfTimeKey;
        [SerializeField] private TextMeshProUGUI remainingTimeText;

        private Coroutine timerTickCoroutine;

        #endregion

        public void Hookup(InterfaceHandle<ILiveOpTimer> timer, long startTimestamp)
        {
            timerTickCoroutine = StartCoroutine(UpdateTimerTick(timer, startTimestamp));
        }

        private IEnumerator UpdateTimerTick(InterfaceHandle<ILiveOpTimer> timer, long startTimestamp)
        {
            long remainingTime = timer.iFace.GetEndTimestamp(timer.instance, startTimestamp) - GameTime.Now;

            while (remainingTime > 0)
            {
                remainingTimeText.text = TimeUtility.FormatTimeString(remainingTime);

                yield return new WaitForSecondsRealtime(1);

                --remainingTime;
            }

            remainingTimeText.text = currentLanguage.Value.Localise(outOfTimeKey);
        }

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref remainingTimeText);

            if (currentLanguage == null)
            {
                currentLanguage = LocalisationEditorSettings.GetOrCreateSettings().currentLanguageValue;
            }
        }

        private void OnDisable()
        {
            if (timerTickCoroutine != null)
            {
                StopCoroutine(timerTickCoroutine);
                timerTickCoroutine = null;
            }
        }

        #endregion
    }
}
