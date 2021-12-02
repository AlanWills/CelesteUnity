using Celeste.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Twine
{
    [Serializable]
    public class TwineNode
    {
        #region Properties and Fields

        public TwineNodeUnityEvent OnChanged { get; } = new TwineNodeUnityEvent();

        public Vector2 Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;
                    OnChanged.Invoke(this);
                }
            }
        }

        public int pid;
        public string name;
        public string text;
        public List<string> tags = new List<string>();
        [SerializeField] private Vector2 position;
        public List<TwineNodeLink> links = new List<TwineNodeLink>();

        #endregion

        public void UpdateData(
            string newName, 
            string newText, 
            string[] newTags, 
            TwineNodeLink[] newLinks)
        {
            name = newName;
            text = newText;
            tags.Clear();
            tags.AddRange(newTags);
            links.Clear();
            links.AddRange(newLinks);

            OnChanged.Invoke(this);
        }
    }
}