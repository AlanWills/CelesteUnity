using Celeste.Events;
using Celeste.Scene.Events;
using System;
using UnityEngine;

namespace Celeste.Twine
{
    [AddComponentMenu("Celeste/Twine/Twine Story Manager")]
    public class TwineStoryManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private TwineStoryEvent saveTwineStory;
        [SerializeField] private TwineNodeEvent onTwineNodeAddedToStory;
        [SerializeField] private TwineNodeEvent onTwineNodeRemovedFromStory;

        [NonSerialized] private TwineStory currentStory;

        #endregion

        #region Callbacks

        public void OnTwineStoryLoaded(TwineStory twineStory)
        {
            if (currentStory != null)
            {
                currentStory.Shutdown();
                saveTwineStory.Invoke(currentStory);
            }

            UnityEngine.Debug.Assert(twineStory != null, $"Null twine story loaded.");
            currentStory = twineStory;
            currentStory.OnNodeAdded.AddListener(OnCurrentStoryNodeAdded);
            currentStory.OnNodeRemoved.AddListener(OnCurrentStoryNodeRemoved);
            currentStory.OnChanged.AddListener(OnCurrentStoryChanged);
        }

        public void OnTwineStoryCreated(TwineStory twineStory)
        {
            OnTwineStoryLoaded(twineStory);
        }

        public void OnSaveCurrentStory()
        {
            UnityEngine.Debug.Assert(currentStory != null, "No current story.  Ignoring SaveCurrentStory...");
            if (currentStory != null)
            {
                saveTwineStory.Invoke(currentStory);
            }
        }

        public void OnAddTwineNode()
        {
            UnityEngine.Debug.Assert(currentStory != null, "No current story.  Ignoring...");
            if (currentStory != null)
            {
                currentStory.AddNode("Untitled");
            }
        }

        public void OnRemoveTwineNode(TwineNode twineNode)
        {
            UnityEngine.Debug.Assert(currentStory != null, "No current story.  Ignoring...");
            if (currentStory != null)
            {
                currentStory.RemoveNode(twineNode);
            }
        }

        private void OnCurrentStoryNodeAdded(TwineNode newNode)
        {
            onTwineNodeAddedToStory.Invoke(newNode);
        }

        private void OnCurrentStoryNodeRemoved(TwineNode newNode)
        {
            onTwineNodeRemovedFromStory.Invoke(newNode);
        }

        private void OnCurrentStoryChanged()
        {
            saveTwineStory.Invoke(currentStory);
        }

        #endregion
    }
}