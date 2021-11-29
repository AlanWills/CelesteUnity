using Celeste.DataStructures;
using Celeste.Events;
using Celeste.Memory;
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

        #endregion

        #region Callbacks

        public void OnTwineStoryLoaded(TwineStory twineStory)
        {
            this.twineStory = twineStory;

            twineNodeUIAllocator.DeallocateAll();

            if (twineStory.passages != null)
            {
                foreach (TwineNode twineNode in twineStory.passages)
                {
                    TwineNodeUIController.From(twineNode, twineNodeUIAllocator);
                }

                // Centre the starting node in the middle of the screen, but adjusting the offset of the parent
                TwineNode startNode = twineStory.passages.Find(x => x.pid == twineStory.startnode);
                UnityEngine.Debug.Assert(startNode != null, $"Could not find start node for pid {twineStory.startnode}.");

                if (startNode != null)
                {
                    twineNodeUIAllocator.transform.localPosition = -startNode.Position;
                }
            }
        }

        public void OnTwineNodeCreated(TwineNode twineNode)
        {
            TwineNodeUIController.From(twineNode, twineNodeUIAllocator);
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