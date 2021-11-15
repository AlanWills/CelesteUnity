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
        public UnityEvent OnChanged { get; } = new UnityEvent();

        public int startnode;
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
                twineNode.OnChanged.AddListener(OnChanged.Invoke);
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

            twineNode.OnChanged.AddListener(OnChanged.Invoke);
            passages.Add(twineNode);

            return twineNode;
        }
    }
}