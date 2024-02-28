using System;
using System.Collections;
using UnityEngine;

namespace Celeste.UI.Layout
{
    [CreateAssetMenu(fileName = nameof(ReferenceLayout), order = CelesteMenuItemConstants.UI_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.UI_MENU_ITEM + "Layout/Reference Layout Rect")]
    public class ReferenceLayout : ScriptableObject
    {
        [NonSerialized] public RectTransform rectTransform;
    }
}