﻿using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/UI/Show Popup Event Listener")]
    public class ShowPopupEventListener : ParameterisedEventListener<IPopupArgs, ShowPopupEvent, ShowPopupUnityEvent>
    {
    }
}
