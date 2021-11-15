using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Twine
{
    [Serializable]
    public class TwineNode
    {
        public UnityEvent OnChanged { get; } = new UnityEvent();

        public Vector2 Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;
                    OnChanged.Invoke();
                }
            }
        }

        public int pid;
        public string name;
        public string text;
        public List<string> tags = new List<string>();
        [SerializeField] private Vector2 position;
        public List<TwineNodeLink> links = new List<TwineNodeLink>();
    }
}