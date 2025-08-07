using System;
using Celeste.Requests;
using Celeste.Tools.Attributes.GUI;
using UnityEngine;

namespace Celeste.Narrative.Requests
{
    [Serializable]
    public struct AnimateBackgroundRequestArgs
    {
        public float AnimationTime;
        public float StartOffset;
        public float FinishOffset;
        public bool UseAnimCurve;
        [ShowIf(nameof(UseAnimCurve))] public AnimationCurve AnimationCurve;
    }
    
    [CreateAssetMenu(fileName = nameof(AnimateBackgroundRequest), menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Requests/Background/Animate Background", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class AnimateBackgroundRequest : ParameterisedRequestWithArgs<AnimateBackgroundRequestArgs>
    {
        public const int BACKGROUND_ANIMATION_ALREADY_IN_PROGRESS_ERROR_CODE = 1;
        public const int NO_BACKGROUND_SET_ERROR_CODE = 2;
    }
}