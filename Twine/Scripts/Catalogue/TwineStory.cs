﻿using Celeste.Events;
using System;
using System.Collections.Generic;
using System.IO;
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
        public TwineNodeUnityEvent OnNodeRemoved { get; } = new TwineNodeUnityEvent();
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

            for (int i = 0, n = passages.Count; i < n; i++)
            {
                TwineNode twineNode = passages[i];
                UnityEngine.Debug.Assert(twineNode != null, $"Null node found in twine story {name} at index {i}.");
                twineNode.Initialize(this);

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
            OnNodeRemoved.RemoveAllListeners();
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
            twineNode.Initialize(this);
            twineNode.Name = name;
            twineNode.pid = ++highestPID;
            twineNode.Position = CalculateBestPosition();
            twineNode.OnChanged.AddListener(OnTwineNodeChanged);

            passages.Add(twineNode);
            
            OnNodeAdded.Invoke(twineNode);
            OnChanged.Invoke();

            return twineNode;
        }

        public void RemoveNode(TwineNode twineNode)
        {
            if (passages.Remove(twineNode))
            {
                OnNodeRemoved.Invoke(twineNode);
                OnChanged.Invoke();
            }
        }

        private Vector2 CalculateBestPosition()
        {
            return passages.Count != 0 ? passages[passages.Count - 1].Position + new Vector2(0, 120) : Vector2.zero;
        }

        #region Save/Load

        public string ToJson()
        {
            return JsonUtility.ToJson(this, true);
        }

        public void Save(string filePath)
        {
            File.WriteAllText(filePath, ToJson());
        }

        #endregion

        #region Callbacks

        private void OnTwineNodeChanged(TwineNode twineNode)
        {
            // Resolve link pids
            foreach (TwineNodeLink twineNodeLink in twineNode.Links)
            {
                TwineNode linkedNode = passages.Find(x => string.CompareOrdinal(x.Name, twineNodeLink.link) == 0);
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