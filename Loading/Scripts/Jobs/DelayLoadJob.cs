﻿using Celeste.Objects;
using Celeste.Parameters;
using Celeste.Tools;
using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Loading
{
    [CreateAssetMenu(fileName = nameof(DelayLoadJob), menuName = CelesteMenuItemConstants.LOADING_MENU_ITEM + "Load Jobs/Delay", order = CelesteMenuItemConstants.LOADING_MENU_ITEM_PRIORITY)]
    public class DelayLoadJob : LoadJob, IEditorInitializable
    {
        #region Properties and Fields

        [SerializeField] private FloatReference loadingDelay;

        #endregion

        public void Editor_Initialize()
        {
            if (loadingDelay == null)
            {
                loadingDelay = CreateInstance<FloatReference>();
                loadingDelay.IsConstant = true;
                loadingDelay.Value = 2f;

                EditorOnly.AddObjectToMainAsset(loadingDelay, this);
            }
        }

        public override IEnumerator Execute(Action<float> setProgress, Action<string> setOutput)
        {
            if (loadingDelay != null)
            {
                yield return new WaitForSeconds(loadingDelay.Value);
            }
        }
    }
}