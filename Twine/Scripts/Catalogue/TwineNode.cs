using Celeste.DataStructures;
using Celeste.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Twine
{
    [Serializable]
    public class TwineNode
    {
        #region Properties and Fields

        public TwineNodeUnityEvent OnChanged { get; private set; } = new TwineNodeUnityEvent();

        public TwineStory TwineStory { get; private set; }
        public bool IsValid { get; private set; }

        public string Name
        {
            get { return name; }
            set
            {
                if (string.CompareOrdinal(name, value) != 0)
                {
                    name = value;
                    NotifyChanged();
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
                    NotifyChanged();
                }
            }
        }

        public IReadOnlyList<string> Tags
        {
            get { return tags; }
            set
            {
                tags.AssignFrom(value);
                NotifyChanged();
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
                    NotifyChanged();
                }
            }
        }

        public IReadOnlyList<TwineNodeLink> Links
        {
            get { return links; }
            set
            {
                links.AssignFrom(value);
                NotifyChanged();
            }
        }

        public int pid;
        [SerializeField] private string name;
        [SerializeField] private string text;
        [SerializeField] private List<string> tags = new List<string>();
        [SerializeField] private Vector2 position;
        [SerializeField] private List<TwineNodeLink> links = new List<TwineNodeLink>();

        #endregion

        public void Initialize(TwineStory twineStory)
        {
            TwineStory = twineStory;

            if (OnChanged != null)
            {
                OnChanged = new TwineNodeUnityEvent();
            }

            Validate();
        }

        public void UpdateData(
            string newName, 
            string newText, 
            IList<string> newTags, 
            IList<TwineNodeLink> newLinks)
        {
            name = newName;
            text = newText;
            tags.AssignFrom(newTags);
            links.AssignFrom(newLinks);

            NotifyChanged();
        }

        public bool Validate()
        {
            bool isValid = true;

            isValid &= !string.IsNullOrEmpty(name);
            isValid &= !string.IsNullOrEmpty(text);
            isValid &= tags.Count > 0;

            for (int i = 0, n = links != null ? links.Count : 0; i < n; ++i)
            {
                isValid &= links[i].Validate(TwineStory);
            }

            IsValid = isValid;
            
            return isValid;
        }

        public void NotifyChanged()
        {
            Validate();

            OnChanged.Invoke(this);
        }
    }
}