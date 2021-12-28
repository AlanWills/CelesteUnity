using System.Collections;
using TMPro;
using UnityEngine;

namespace Celeste.UI.Skin
{
    [CreateAssetMenu(fileName = nameof(UISkin), menuName = "Celeste/UI/Skin/UI Skin")]
    public class UISkin : ScriptableObject
    {
        public TMP_FontAsset font;
    }
}