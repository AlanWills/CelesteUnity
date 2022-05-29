using Celeste.Tools.Attributes.GUI;
using System;
using UnityEngine;

namespace Celeste.Components
{
    public class ComponentTemplate : ScriptableObject
    {
        public Component component;
        [SerializeReference, InlineDataInInspector] public ComponentData componentData;
    }
}
