#if USE_LUA
using System;
using Celeste.Events;
using Celeste.Lua;
using Celeste.Tools;
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

        public void Show(IPopupArgs args)
        {
            ExecuteLuaFunction(showFunctionName).FireAndForget($"{name}.{nameof(Show)}");
        }

        public void Hide()
        {
            ExecuteLuaFunction(hideFunctionName).FireAndForget($"{name}.{nameof(Hide)}");
        }

        public void ConfirmPressed()
        {
            ExecuteLuaFunction(confirmPressedFunctionName).FireAndForget($"{name}.{nameof(ConfirmPressed)}");
        }

        public void ClosePressed()
        {
            ExecuteLuaFunction(closePressedFunctionName).FireAndForget($"{name}.{nameof(ClosePressed)}");
        }
    }
}
#endif