#if USE_LUA
using System;
using Celeste.Events;
using Celeste.Lua;
using Celeste.Tools.Attributes.GUI;
using UnityEngine;

namespace Celeste.UI
{
    public class LuaScriptPopupController : LuaScriptMonoBehaviour, IPopupController
    {
        #region Properties and Fields
        
        public Action RequestToClosePopup { get; set; }

        [SerializeField, ShowIf(nameof(showAdvancedSettings))] private string showFunctionName = "show";
        [SerializeField, ShowIf(nameof(showAdvancedSettings))] private string hideFunctionName = "hide";
        [SerializeField, ShowIf(nameof(showAdvancedSettings))] private string confirmPressedFunctionName = "confirmPressed";
        [SerializeField, ShowIf(nameof(showAdvancedSettings))] private string closePressedFunctionName = "closePressed";
        
        #endregion

        public async void Show(IPopupArgs args)
        {
            await ExecuteLuaFunction(showFunctionName);
        }

        public async void Hide()
        {
            await ExecuteLuaFunction(hideFunctionName);
        }

        public async void ConfirmPressed()
        {
            await ExecuteLuaFunction(confirmPressedFunctionName);
        }

        public async void ClosePressed()
        {
            await ExecuteLuaFunction(closePressedFunctionName);
        }
    }
}
#endif