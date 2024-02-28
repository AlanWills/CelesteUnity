using Celeste.Constants;
using Celeste.DataStructures;
using Celeste.Tools.Attributes.GUI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Celeste.UI.Skin
{
    [Serializable]
    public struct UISkinTextSettings
    {
        public bool IsValid => fontAsset != null;

        public ID fontType;
        public TMP_FontAsset fontAsset;
    }

    [CreateAssetMenu(fileName = nameof(UISkin), order = CelesteMenuItemConstants.UI_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.UI_MENU_ITEM + "Skin/UI Skin")]
    public class UISkin : ScriptableObject
    {
        #region Properties and Fields

        [SerializeField, InlineDataInInspector] private UISkinTextSettings fallbackTextSettings;
        [SerializeField] private List<UISkinTextSettings> textSettings = new List<UISkinTextSettings>();

        #endregion

        public void ApplyTextSettings(TextMeshProUGUI textMeshProUGUI, ID textType)
        {
            UISkinTextSettings textSettings = FindTextSettings(textType);

            if (textMeshProUGUI.font != textSettings.fontAsset)
            {
                textMeshProUGUI.font = textSettings.fontAsset;
            }
        }

        private UISkinTextSettings FindTextSettings(ID fontType)
        {
            if (fontType == null)
            {
                return fallbackTextSettings;
            }

            var textSettings = this.textSettings.Find(x => x.fontType == fontType);
            return textSettings.IsValid ? textSettings : fallbackTextSettings;
        }
    }
}