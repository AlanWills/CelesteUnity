using Celeste.Events;
using System;
using UnityEngine;

namespace Celeste.Twine
{
    [AddComponentMenu("Celeste/Twine/Twine Story Manager")]
    public class TwineStoryManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private TwineStoryEvent saveTwineStory;
        [SerializeField] private TwineNodeEvent createdTwineNode;

        [NonSerialized] private TwineStory currentStory;

        #endregion

        private Vector2 CalculateBestPosition()
        {
            var passages = currentStory.passages;
            return passages.Count != 0 ? passages[passages.Count - 1].Position + new Vector2(0, 120) : Vector2.zero;
        }

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
            currentStory.OnChanged.AddListener(OnCurrentStoryChanged);
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
            UnityEngine.Debug.Assert(currentStory != null, "No current story.  Ignoring SaveCurrentStory...");
            if (currentStory != null)
            {
                // Calculate this before we add the node, otherwise it'll use the new node
                Vector2 bestPosition = CalculateBestPosition();
                TwineNode twineNode = currentStory.AddNode("Untitled");
                twineNode.Position = bestPosition;

                createdTwineNode.Invoke(twineNode);
            }
        }

        private void OnCurrentStoryChanged()
        {
            saveTwineStory.Invoke(currentStory);
        }

        #endregion
    }
}