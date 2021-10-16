using System;
using UnityEditor;
using UnityEngine;

namespace XNode.Attributes
{
    public class NodeGraphShortcutAttribute : Attribute
    {
        public KeyCode Key { get; }
        public EventModifiers Modifiers { get; }

        public NodeGraphShortcutAttribute(
            KeyCode key,
            EventModifiers modifiers = EventModifiers.Control)
        {
            Key = key;
            Modifiers = modifiers;
        }
    }
}