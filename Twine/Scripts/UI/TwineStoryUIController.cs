using Celeste.Events;
using Celeste.Memory;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Twine.UI
{
    [AddComponentMenu("Celeste/Twine/UI/Twine Story UI Controller")]
    public class TwineStoryUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private GameObjectAllocator twineNodeUIAllocator;
        [SerializeField] private ShowPopupEvent showEditTwineNodePopup;

        private TwineStory twineStory;
        private List<TwineNodeUIController> twineNodeUIControllers = new List<TwineNodeUIController>();

        #endregion

        private void CentreOn(TwineNode twineNode)
        {
            twineNodeUIAllocator.transform.localPosition = -twineNode.Position;
        }

        #region Callbacks

        public void OnTwineStoryLoaded(TwineStory twineStory)
        {
            this.twineStory = twineStory;

            twineNodeUIControllers.Clear();
            twineNodeUIAllocator.DeallocateAll();

            if (twineStory.passages != null)
            {
                foreach (TwineNode twineNode in twineStory.passages)
                {
                    twineNodeUIControllers.Add(TwineNodeUIController.From(twineNode, twineNodeUIAllocator));
                }

                // Centre the last created node in the middle of the screen by adjusting the offset of the parent
                int passagesCount = twineStory.passages.Count;
                if (passagesCount > 0)
                {
                    CentreOn(twineStory.passages[passagesCount - 1]);
                }
            }
        }

        public void OnTwineStoryCreated(TwineStory twineStory)
        {
            OnTwineStoryLoaded(twineStory);
        }

        public void OnTwineNodeAdded(TwineNode twineNode)
        {
            twineNodeUIControllers.Add(TwineNodeUIController.From(twineNode, twineNodeUIAllocator));
            CentreOn(twineNode);
        }
        
        public void OnTwineNodeRemoved(TwineNode twineNode)
        {
            TwineNodeUIController twineNodeUIController = twineNodeUIControllers.Find(x => x.IsFor(twineNode));
            if (twineNodeUIController != null)
            {
                twineNodeUIControllers.Remove(twineNodeUIController);
                twineNodeUIAllocator.Deallocate(twineNodeUIController.gameObject);
            }
        }

        public void OnFollowTwineNodeLink(int pid)
        {
            TwineNode followLinkNode = twineStory.passages.Find(x => x.pid == pid);
            UnityEngine.Debug.Assert(followLinkNode != null, $"Could not find follow link node with pid: {pid}.");

            if (followLinkNode != null)
            {
                showEditTwineNodePopup.Invoke(new EditTwineNodePopupController.EditTwineNodePopupArgs()
                {
                    twineNode = followLinkNode
                });
            }
        }

        #endregion
    }
}