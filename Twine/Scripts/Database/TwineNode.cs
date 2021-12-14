using Celeste.DataStructures;
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

        public string Name
        {
            get { return name; }
            set
            {
                if (string.CompareOrdinal(name, value) != 0)
                {
                    name = value;
                    OnChanged.Invoke(this);
                }
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                if (string.CompareOrdinal(text, value) != 0)
                {
                    text = value;
                    OnChanged.Invoke(this);
                }
            }
        }

        public List<string> Tags
        {
            get { return tags; }
            set
            {
                tags.AssignFrom(value);
                OnChanged.Invoke(this);
            }
        }

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

        public List<TwineNodeLink> Links
        {
            get { return links; }
            set
            {
                links.AssignFrom(value);
                OnChanged.Invoke(this);
            }
        }

        public int pid;
        [SerializeField] private string name;
        [SerializeField] private string text;
        [SerializeField] List<string> tags = new List<string>();
        [SerializeField] private Vector2 position;
        [SerializeField] private List<TwineNodeLink> links = new List<TwineNodeLink>();

        #endregion

        public void UpdateData(
            string newName, 
            string newText, 
            string[] newTags, 
            TwineNodeLink[] newLinks)
        {
            name = newName;
            text = newText;
            tags.AssignFrom(newTags);
            links.AssignFrom(newLinks);

            OnChanged.Invoke(this);
        }
    }
}