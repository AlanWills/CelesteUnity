using Celeste.Assets;
using Celeste.Logic.Settings;
using System.Collections;
using UnityEngine;

namespace Celeste.Logic
{
    [AddComponentMenu("Celeste/Logic/Condition Manager")]
    public class ConditionManager : MonoBehaviour, IHasAssets
    {
        #region Properties and Fields

        [SerializeField] private ConditionSettings conditionSettings;

        #endregion

        #region IHasAssets

        public bool ShouldLoadAssets()
        {
            return conditionSettings.ShouldLoadAssets();
        }

        public IEnumerator LoadAssets()
        {
            yield return conditionSettings.LoadAssets();
        }

        #endregion

        #region Unity Methods

        private void Start()
        {
            foreach (var condition in conditionSettings.Conditions)
            {
                condition.Initialize();
            }
        }

        private void OnDestroy()
        {
            foreach (var condition in conditionSettings.Conditions)
            {
                condition.Shutdown();
            }
        }

        #endregion
    }
}
