using Celeste.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Twine
{
    [Serializable]
    public class TwineStory : ScriptableObject
    {
        #region Properties and Fields

        public const string FILE_EXTENSION = "twine";

        public TwineNodeUnityEvent OnNodeAdded { get; } = new TwineNodeUnityEvent();
        public UnityEvent OnChanged { get; } = new UnityEvent();

        public List<TwineNode> passages = new List<TwineNode>();

        [NonSerialized] private bool isInitialized = false;
        [NonSerialized] private int highestPID = 0;

        #endregion

        public void Initialize()
        {
            if (isInitialized)
            {
                UnityEngine.Debug.LogError($"Twine Story {name} is already initialized.");
                return;
            }

            foreach (TwineNode twineNode in passages)
            {
                highestPID = Mathf.Max(twineNode.pid, highestPID);
                twineNode.OnChanged.AddListener(OnTwineNodeChanged);
            }

            isInitialized = true;
        }

        public void Shutdown()
        {
            if (!isInitialized)
            {
                UnityEngine.Debug.LogError($"Twine Story {name} is already shutdown.");
                return;
            }

            OnNodeAdded.RemoveAllListeners();
            OnChanged.RemoveAllListeners();

            foreach (TwineNode twineNode in passages)
            {
                twineNode.OnChanged.RemoveAllListeners();
            }

            isInitialized = false;
        }

        public TwineNode AddNode(string name)
        {
            TwineNode twineNode = new TwineNode();
            twineNode.name = name;
            twineNode.pid = ++highestPID;
            twineNode.Position = CalculateBestPosition();

            twineNode.OnChanged.AddListener(OnTwineNodeChanged);
            passages.Add(twineNode);
            
            OnNodeAdded.Invoke(twineNode);
            OnChanged.Invoke();

            return twineNode;
        }

        private Vector2 CalculateBestPosition()
        {
            return passages.Count != 0 ? passages[passages.Count - 1].Position + new Vector2(0, 120) : Vector2.zero;
        }

        #region Callbacks

        private void OnTwineNodeChanged(TwineNode twineNode)
        {
            // Resolve link pids
            foreach (TwineNodeLink twineNodeLink in twineNode.links)
            {
                TwineNode linkedNode = passages.Find(x => string.CompareOrdinal(x.name, twineNodeLink.link) == 0);
                if (linkedNode == null)
                {
                    // Calculate best position before adding node to ensure it calculates the best position
                    // based on the old nodes
                    linkedNode = AddNode(twineNodeLink.link);
                }

                twineNodeLink.pid = linkedNode.pid;
            }

            OnChanged.Invoke();
        }

        #endregion
    }
}