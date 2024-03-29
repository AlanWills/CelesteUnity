using Celeste.Parameters;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Loading
{
    [CreateAssetMenu(fileName = nameof(DisableFallbackLoadAssetsLoadJob), menuName = CelesteMenuItemConstants.LOADING_MENU_ITEM + "Load Jobs/Disable Fallback Load Assets", order = CelesteMenuItemConstants.LOADING_MENU_ITEM_PRIORITY)]
    public class DisableFallbackLoadAssetsLoadJob : LoadJob
    {
        #region Properties and Fields

        [SerializeField] private BoolValue shouldFallbackLoadAssets;

        #endregion

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            shouldFallbackLoadAssets.Value = false;

            yield break;
        }
    }
}